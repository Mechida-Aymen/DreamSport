using System.Net;
using System.Text;
using System.Text.Json;

namespace ApiGateway.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;

        public TenantMiddleware(RequestDelegate next, HttpClient httpClient)
        {
            _next = next;
            _httpClient = httpClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            // Vérifie si Tenant-ID est présent
            if (!context.Request.Headers.TryGetValue("Tenant-ID", out var tenantIdString))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant-ID is missing.");
                return;
            }

            // Vérifie si Tenant-ID est un entier valide
            if (!int.TryParse(tenantIdString, out int tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant-ID must be a valid integer.");
                return;
            }

            try
            {

                // Appel au service admin pour valider le Tenant-ID
                var response = await _httpClient.GetAsync($"http://gestionemployer:8080/api/admin/validate/{tenantId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid Tenant-ID.");
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsync("Tenant service unavailable.");
                    return;
                }

                context.Items["Tenant-ID"] = tenantId;
                // Modifier le corps de la requête pour POST, comme avant
                if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put && context.Request.ContentType == "application/json")
                {
                    context.Request.EnableBuffering();
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                    {
                        var existingBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;


                        // Modifier le JSON pour ajouter AdminId
                        var jsonDocument = JsonDocument.Parse(existingBody);
                        var jsonObject = jsonDocument.RootElement.Clone();
                        var modifiedJson = JsonSerializer.Serialize(jsonObject);

                        using (var ms = new MemoryStream())
                        {
                            using (var writer = new Utf8JsonWriter(ms))
                            {
                                writer.WriteStartObject();
                                foreach (var property in jsonObject.EnumerateObject())
                                {
                                    property.WriteTo(writer);
                                }
                                writer.WriteNumber("AdminId", tenantId);
                                writer.WriteEndObject();
                            }

                            modifiedJson = Encoding.UTF8.GetString(ms.ToArray());
                        }


                        // Réécrire le body avec AdminId
                        var newBody = new MemoryStream(Encoding.UTF8.GetBytes(modifiedJson));
                        context.Request.Body = newBody;
                        context.Request.ContentLength = newBody.Length;
                        context.Request.Body.Position = 0;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Tenant service unavailable.");
                return;
            }

            await _next(context);
        }

    }
}
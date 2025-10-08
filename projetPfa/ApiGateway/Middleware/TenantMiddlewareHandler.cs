using ApiGateway.Middleware;

public class TenantMiddlewareHandler
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _httpClient;

    public TenantMiddlewareHandler(RequestDelegate next, HttpClient httpClient)
    {
        _next = next;
        _httpClient = httpClient;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = context.Request.Headers["Tenant-ID"].ToString();


        if (context.Request.Method == HttpMethods.Get || context.Request.Method == HttpMethods.Delete)
        {
            var path = context.Request.Path.ToString();

            // Vérifier si l'URL contient {AdminId}
            if (path.Contains("{AdminId}"))
            {
                // Remplacer {AdminId} par tenantId dans le chemin de la requête
                var newPath = path.Replace("{AdminId}", tenantId.ToString());
                context.Request.Path = new PathString(newPath);
            }
            else
            {
                // Si le {AdminId} n'est pas présent, ajouter un ID par défaut (par exemple tenantId comme ID)
                var newPath = path + "/" + tenantId.ToString();
                context.Request.Path = new PathString(newPath);
            }

        }

        // Passer la requête au middleware suivant (Ocelot ou autre)
        await _next(context);
    }
}
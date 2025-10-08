using System.IdentityModel.Tokens.Jwt;

namespace ApiGateway.Middleware
{
    public class TenantComparingMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantComparingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var hasToken = context.Request.Headers.TryGetValue("Authorization", out var authHeader);
            var hasTenantHeader = context.Request.Headers.TryGetValue("Tenant-ID", out var tenantIdHeader);

            if (hasToken && hasTenantHeader)
            {
                var token = authHeader.ToString().Replace("Bearer ", "");

                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var idAdminClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "AdminId");

                    if (idAdminClaim != null && idAdminClaim.Value != tenantIdHeader)
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Tenant-ID does not match idAdmin in token.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}

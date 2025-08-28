namespace PMSApi.Middleware
{
    public class ResponseHeadersMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            context
                .Response
                .OnStarting(() =>
                {
                    if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
                        context
                            .Response
                            .Headers
                            .Append("Content-Security-Policy", "default-src 'self'; ");

                    if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
                        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

                    if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
                        context.Response.Headers.Append("X-Frame-Options", "DENY");

                    if (!context.Response.Headers.ContainsKey("X-Xss-Protection"))
                        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");

                    if (!context.Response.Headers.ContainsKey("Referrer-Policy"))
                        context.Response.Headers.Append("Referrer-Policy", "no-referrer");

                    return Task.CompletedTask;
                });

            await _next(context);
        }
    }

}

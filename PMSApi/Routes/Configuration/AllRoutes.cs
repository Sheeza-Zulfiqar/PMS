namespace PMSApi.Routes.Configuration
{
    public static class AllRoutes
    {
        public static WebApplication AddAllRoutes(this WebApplication app)
        {
 
            app.MapUserRoutes();
            app.MapProjectRoutes();

            return app;
        }
    }
}

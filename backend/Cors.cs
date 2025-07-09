namespace CorsRules;

public class Cors()
{
    public static WebApplicationBuilder Add(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins("http://localhost:9000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return builder;
    }
    public static WebApplication Start(WebApplication app)
    {
        app.UseCors("CorsPolicy");
        
        return app;
    }
    
}
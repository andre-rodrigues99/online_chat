using CorsRules;

namespace backend;

public class Program
{
    public static void Main(string[] args)
    {
        //var mig = new OracleDb();
        //mig.RunMigration();

        WebApplication app = BuildApp();
        ConfigureApp(app);
        app.Run();
    }

    private static WebApplication BuildApp()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();

        builder = Cors.Add(builder);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    private static WebApplication ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        var webSocketOptions = new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        app.UseWebSockets(webSocketOptions);

        app.MapControllers();

        app = Cors.Start(app);

        return app;
    }
}

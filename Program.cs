// path: /Program.cs

using dotenv.net;
using Serilog;

using Neurocache.Csharp.Nexus.NodeRouter;

IDictionary<string, string>? envVars = null;
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    envVars = DotEnv.Fluent()
        .WithExceptions()
        .WithEnvFiles()
        .WithTrimValues()
        .WithOverwriteExistingVars()
        .WithProbeForEnv(probeLevelsToSearch: 6)
        .Read();
}
else
{
    envVars = new Dictionary<string, string>
    {
        { "BETTERSTACK_LOGS_SOURCE_TOKEN", Environment.GetEnvironmentVariable("BETTERSTACK_LOGS_SOURCE_TOKEN")! }
    };
}

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.BetterStack(sourceToken: envVars["BETTERSTACK_LOGS_SOURCE_TOKEN"])
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();
{
    app.MapControllers();

    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        BulletinRouter.Init();
        Log.Information("<--- Csharp Nexus Started --->");
    });
    lifetime.ApplicationStopping.Register(() =>
    {
        Log.Information("<--- Csharp Nexus Stopped --->");
        Log.CloseAndFlush();
    });

    app.Run();
}

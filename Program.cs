// path: /Program.cs

using dotenv.net;
using Serilog;

var envVars = DotEnv.Fluent()
    .WithExceptions()
    .WithEnvFiles()
    .WithTrimValues()
    .WithOverwriteExistingVars()
    .WithProbeForEnv(probeLevelsToSearch: 6)
    .Read();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRouter>();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.BetterStack(sourceToken: envVars["BETTERSTACK_LOGS_SOURCE_TOKEN"])
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();

    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information("<--- Csharp Nexus Started --->");
    });
    lifetime.ApplicationStopping.Register(() =>
    {
        Log.Information("<--- Csharp Nexus Stopped --->");
        Log.CloseAndFlush();
    });

    app.Run();
}

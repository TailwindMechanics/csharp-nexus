// path: /Program.cs

using dotenv.net;
using Serilog;

using Neurocache.LogkeepFrigate;
using Neurocache.Lifetime;
using Neurocache.Hubs;

var builder = WebApplication.CreateBuilder(args);
{
    DotEnv.Fluent().WithEnvFiles().WithOverwriteExistingVars()
        .WithProbeForEnv(probeLevelsToSearch: 6).Load();

    var port = Environment.GetEnvironmentVariable("PORT");
    builder.WebHost.UseUrls($"http://*:{port}");
    builder.Services.AddControllers();

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Logkeep.SystemLogger());
    Log.Logger = Logkeep.ShipLogger();
}

var app = builder.Build();
{
    app.MapControllers();
    new Lifetime().Subscribe(app.Services, () =>
    {
        HubOperationService.Init();
    });
    app.Run();
}

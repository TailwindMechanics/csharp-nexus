// path: Program.cs

using dotenv.net;
using Serilog;

using Neurocache.RequestsChannel;
using Neurocache.ConduitFrigate;
using Neurocache.LogkeepFrigate;
using Neurocache.Operations;
using Neurocache.Lifetime;

var builder = WebApplication.CreateBuilder(args);
{
    DotEnv.Fluent().WithEnvFiles().WithOverwriteExistingVars()
        .WithProbeForEnv(probeLevelsToSearch: 6).Load();

    // Configure web server
    var port = Environment.GetEnvironmentVariable("PORT");
    builder.WebHost.UseUrls($"http://*:{port}");
    builder.Services.AddControllers();

    // Register services
    builder.Services.AddSingleton(_ => Conduit.UplinkProducer);
    builder.Services.AddSingleton(_ => Conduit.DownlinkConsumer);

    // Logging
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Logkeep.SystemLogger());
    Log.Logger = Logkeep.ShipLogger();
}

var app = builder.Build();
{
    app.UseWebSockets();
    app.MapControllers();
    new Lifetime().Subscribe(app.Services,
    () =>
    {
        RequestsChannelService.OnAppStart();
        OperationService.OnAppStarted();
    },
    () =>
    {
        RequestsChannelService.OnAppClosing();
        OperationService.OnAppClosing();
    });
    app.Run();
}

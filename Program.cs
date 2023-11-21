// path: /Program.cs

using Serilog;

using Neurocache.LogbookFrigate;
using Neurocache.NodeRouter;
using Neurocache.Lifetime;
using Neurocache.Envars;

IDictionary<string, string> envVars = EnvironmentVariables.Init(
    [
        "BETTERSTACK_LOGS_SOURCE_TOKEN"
    ]
);

var builder = WebApplication.CreateBuilder(args);
{
    var port = Environment.GetEnvironmentVariable("PORT");
    builder.WebHost.UseUrls($"http://*:{port}");
    builder.Services.AddControllers();
}

var app = builder.Build();
{
    app.MapControllers();
    new Lifetime().Subscribe(app.Services, BulletinRouter.Init);
    app.Run();
}

Log.Logger = Logbook.CreateLogger(envVars);
//path: src\ShipsInfo\Ships.cs

using Neurocache.Schema;

namespace Neurocache.ShipsInfo
{
    public static class Ships
    {
        public static void Log(string message)
            => Serilog.Log.Information($"==> {message}");

        public static readonly string FleetName = "neurocache_fleet";

        public static Ship ThisVessel
            => new("dotnet_cruiser", ThisVesselPort()!.Value);

        public static string SocketAddress(Ship ship)
            => $"ws://{ship.Name}.neurocache.koyeb:{ship.Port}"
                .Replace("_", "-");

        public static Ship VanguardStarship
            => new("dotnet_vanguard_starship", 5001);

        public static readonly List<Ship> Cruisers = [
            new Ship("dotnet_cruiser", 5050),
        ];

        public static string VesselAddress(Ship ship)
            => $"http://{ship.Name}.neurocache.koyeb:{ship.Port}"
                .Replace("_", "-");

        public static int? ThisVesselPort()
        {
            var portString = Environment.GetEnvironmentVariable("PORT");
            if (portString == null)
            {
                Log("PORT environment variable not found");
                return null;
            }

            if (!int.TryParse(portString, out var port))
            {
                Log("PORT environment variable is not a valid port");
                return null;
            }

            return port;
        }
    }
}

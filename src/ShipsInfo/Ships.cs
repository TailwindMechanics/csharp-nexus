//path: src\ShipsInfo\Ships.cs

namespace Neurocache.ShipsInfo
{
    public static class Ships
    {
        public static readonly string ThisVessel = "Dotnet Cruiser";
        public static readonly string ThisVesselId = "dotnet_cruiser";

        public static readonly string FleetName = "neurocache_fleet";
        public static readonly string VanguardName = "dotnet_vanguard_gateway";

        public static void Log(string message)
            => Serilog.Log.Information($"==> {message}");

        public static void Warning(string message)
            => Serilog.Log.Warning($"==> {message}");
    }
}

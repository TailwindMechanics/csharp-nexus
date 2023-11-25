//path: src\Utilities\HubUtils.cs

using Neurocache.Schema;

namespace Neurocache.Utilities
{
    public static class Utils
    {
        public static Task Wait(float time, HubOperation hubOperation)
            => Task.Delay(TimeSpan.FromSeconds(time),
                hubOperation.CancelToken.Token);
    }
}
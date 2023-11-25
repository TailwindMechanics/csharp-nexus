//path: src\Schema\INode.cs

namespace Neurocache.Schema
{
    public interface IHub
    {
        static abstract string HubId { get; }
        static abstract void Run(HubOperation hubOperation);
    }
}
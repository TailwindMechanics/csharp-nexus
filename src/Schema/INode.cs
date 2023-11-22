//path: src\Schema\INode.cs

namespace Neurocache.Schema
{
    public interface INode
    {
        static abstract string NodeId { get; }
        static abstract void Run(HubOperation hubOperation);
    }
}
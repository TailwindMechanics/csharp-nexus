//path: src\Schema\INode.cs

using System.Reactive.Subjects;

namespace Neurocache.Csharp.Nexus.Schema
{
    public interface INode
    {
        static abstract string NodeId { get; }
        static abstract void Run(string sessionToken, string payload, ISubject<NodeRecord> callback);
    }
}
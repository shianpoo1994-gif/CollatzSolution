using System.Numerics;

namespace Collatz.Api.Contracts
{
    public class CollatzResponse
    {
        public bool Ok { get; set; }
        public string? OddOps { get; set; }
        public string? EvenOps { get; set;}
        public string? MaxValue { get; set; }
        public string? Error { get; set; }
    }
}

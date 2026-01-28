using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Collatz.Core
{
    public sealed class CollatzResult
    {
        public BigInteger OddOps { get; init; }
        public BigInteger EvenOps { get; init; }

        public BigInteger MaxValue { get; init; }
    }
}

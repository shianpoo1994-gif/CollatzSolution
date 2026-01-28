using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Collatz.Core
{
    public sealed class CollatzCalculator
    {
        public CollatzResult Calculate(BigInteger n) {
            if (n <= 0) 
                throw new ArgumentOutOfRangeException(nameof(n));
            BigInteger oddOps = 0;
            BigInteger evenOps = 0;
            var max = n;
            var current = n;

            while (current != 1)
            {
                if (current > max) max = current;
                if (current.IsEven)
                {
                    current /= 2;
                    evenOps++;
                }
                else
                {
                    current = current * 3 + 1;
                    oddOps++;
                }
            }

            return new CollatzResult
            {
                OddOps = oddOps,
                EvenOps = evenOps,
                MaxValue = max
            };
          }
        }
}

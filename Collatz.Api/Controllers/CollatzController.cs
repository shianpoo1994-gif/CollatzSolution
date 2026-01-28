using Microsoft.AspNetCore.Mvc;
using Collatz.Core;
using Collatz.Api.Contracts;
using System.Numerics;

namespace Collatz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollatzController : ControllerBase
    {
        private static readonly CollatzCalculator _calc = new CollatzCalculator();

        [HttpPost]
        public ActionResult<CollatzResponse> Post([FromBody] CollatzRequest req)
        {
            if (!NaturalNumberValidator.TryParse(req.N, out BigInteger n, out var error)) {
                return BadRequest(new CollatzResponse { Ok = false, Error = error });
            }
            var result = _calc.Calculate(n);
            return Ok(new CollatzResponse {
                Ok = true,
                OddOps = result.OddOps.ToString(),
                EvenOps = result.EvenOps.ToString(),
                MaxValue = result.MaxValue.ToString() 
            });
        }
    }
}

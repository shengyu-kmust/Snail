using Microsoft.AspNetCore.Mvc;

namespace Snail.Pay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        [HttpGet,HttpPost]
        public object Notify()
        {
            return null;
        }

        [HttpGet,HttpPost]
        public object ToPay()
        {
            return null;
        }
    }
}

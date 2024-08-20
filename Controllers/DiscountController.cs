using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskAuthenticationAuthorization.Controllers
{
    [Authorize(Policy = "GoldenWholesaleOnly")]
    public class DiscountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

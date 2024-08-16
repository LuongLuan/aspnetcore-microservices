using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    public class HomeController : Controller
    {
        
        // GET
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}

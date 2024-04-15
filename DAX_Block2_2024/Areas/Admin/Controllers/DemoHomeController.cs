using Microsoft.AspNetCore.Mvc;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    public class DemoHomeController : Controller
    {
        [Area("Admin")]
        [Route("/")]

        public IActionResult Index()
        {
            return View();
        }
    }
}

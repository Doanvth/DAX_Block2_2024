using DAX_Block2_2024.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAX_Block2_2024.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        [Authentication]

        public IActionResult Index()
        {
            return View();
        }
    }
}

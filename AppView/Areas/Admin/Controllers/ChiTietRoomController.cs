using Microsoft.AspNetCore.Mvc;

namespace AppView.Areas.Admin.Controllers
{
    
    public class ChiTietRoomController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

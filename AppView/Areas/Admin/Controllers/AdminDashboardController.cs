using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppView.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public AdminDashboardController()
		{
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var session = context.HttpContext.Session.GetString("TaiKhoan");
			var role = context.HttpContext.Session.GetString("Quyen");

			// Kiểm tra nếu chưa đăng nhập hoặc không phải Admin
			if (string.IsNullOrEmpty(session) || role != "admin")
			{
				context.Result = RedirectToAction("Login", "Login", new { area = "" });
			}

			base.OnActionExecuting(context);
		}
	}
}

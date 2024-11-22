using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AppView.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public FooterViewComponent(HotelDbContext context) => db = context;
        public IViewComponentResult Invoke()
        {
            var data = db.lienHes.Select(x => new LienHeVM
            {
              
               
            });
            return View(data);
        }
    }
}

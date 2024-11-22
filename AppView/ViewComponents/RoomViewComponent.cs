using AppData;
using AppView.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppView.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private readonly HotelDbContext db;

        public RoomViewComponent(HotelDbContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            // Truy vấn dữ liệu Loại Phòng và các Dịch Vụ liên quan
            var data = db.LoaiPhongs
                .Include(lp => lp.DichVus) // Bao gồm các dịch vụ
                .Select(lp => new LoaiPhongVM
                {
                    MaLoaiPhong = lp.MaLoaiPhong,
                    TenLoaiPhong = lp.TenLoaiPhong,
                    MoTa = lp.MoTa,
                    Anh = lp.Anh,
                    GiaGoc = lp.GiaGoc,
                    GiaGiamGia = lp.GiaGiamGia,
                    DichVus = lp.DichVus.Select(dv => new DichVuVM
                    {
                        Ten = dv.Ten,
                        MoTa = dv.MoTa,
                        Hinh = dv.Hinh
                    }).ToList()
                }).ToList();

            return View(data);
        }

    }
}

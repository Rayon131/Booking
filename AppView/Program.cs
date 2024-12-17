using AppData;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Th?i gian h?t h?n Session
    options.Cookie.HttpOnly = true; // Ch? có th? truy c?p Session t? server
    options.Cookie.IsEssential = true; // Là cookie quan tr?ng
});
builder.Services.AddControllersWithViews();
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;  // ?i?u ch?nh kích th??c t?i ?a t?p
    options.MultipartBodyLengthLimit = int.MaxValue; // T?ng kích th??c t?p t?i ?a cho multipart
});
builder.Services.AddHttpClient();

builder.Services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();  // Thêm h? tr? Session
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseRouting();

app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

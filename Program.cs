using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CatForum.Data;
using CatForum.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatForumContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatForumContext"))); 

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; 
})
    .AddEntityFrameworkStores<CatForumContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); 

app.Run();

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString))
                .AddIdentity<DbUser, IdentityRole>(conf => { 
                                    conf.Password.RequireNonAlphanumeric = false;
                                    conf.Password.RequireUppercase = false; })
                .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddDefaultIdentity<DbUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(conf =>{
                                                        conf.LoginPath = "/Account/Login";
                                                        conf.AccessDeniedPath = "/Home/AccesDenied";
                                                   });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "Manager");
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("", builder =>
    {
        builder.RequireAssertion(x => x.User.HasClaim(ClaimTypes.Role, "Manager")
                                   || x.User.HasClaim(ClaimTypes.Role, "Administrator"));
    });
});

//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddRazorPages();   
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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

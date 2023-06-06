using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;
using FluentValidation;
using VacaYAY.Business.Validators;
using VacaYAY.Business.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
});

builder.Services.AddDbContext<VacayayDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("vacayay-db"));
});

builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<VacayayDbContext>();

builder.Services.AddScoped<SignInManager<Employee>, SignInManager<Employee>>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();

builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<IJsonParserService, JsonParserService>();

builder.Services.AddHttpClient(nameof(IHttpService), httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:5110");
});


builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

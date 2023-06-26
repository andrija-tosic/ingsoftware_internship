using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VacaYAY.Business;
using VacaYAY.Data;
using VacaYAY.Data.Models;
using VacaYAY.Business.Services;
using SendGrid;
using Hangfire;
using Microsoft.Extensions.Azure;

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
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<VacayayDbContext>();

builder.Services.AddScoped<SignInManager<Employee>, SignInManager<Employee>>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IHttpService, HttpService>();
builder.Services.AddSingleton<IJsonParserService, JsonParserService>();

builder.Services.AddHttpClient(nameof(IHttpService), httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:5110");
});

builder.Services.AddSingleton<ISendGridClient>(provider =>
    new SendGridClient(new SendGridClientOptions
    {
        ApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? builder.Configuration["SendGrid:ApiKey"],
        HttpErrorAsException = true
    })
);

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("vacayay-db")));

builder.Services.AddHangfireServer();

builder.Services.AddRazorPages();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["StorageConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["StorageConnectionString:queue"]!, preferMsi: true);
});

var app = builder.Build();


if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<VacayayDbContext>();
        context.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHangfireDashboard();

app.MapRazorPages();

app.Run();

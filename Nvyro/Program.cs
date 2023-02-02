using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Nvyro.Data;
using Nvyro.Models;
using Nvyro.Services;
using AspNetCore.ReCaptcha;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("MyDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MyDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NToastNotify.NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddReCaptcha(builder.Configuration.GetSection("GoogleRecaptcha"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=>{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(36500);

}).AddEntityFrameworkStores<MyDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/User/Login";
});

builder.Services.AddSingleton<EmailSender>();
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<RewardService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.UseNToastNotify();
app.UseNotyf();

app.MapRazorPages();
app.MapControllers();

app.Run();

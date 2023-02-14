using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Nvyro.Data;
using Nvyro.Models;
using Nvyro.Services;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddReCaptcha(options =>
{
    options.SiteKey = builder.Configuration["GoogleRecaptcha:GoogleCaptchaSitKey"];
    options.SecretKey = builder.Configuration["GoogleRecaptcha:GoogleCaptchaSecretKey"];
    options.Version = ReCaptchaVersion.V3;
    options.ScoreThreshold = 0.5;
});
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
    Config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    Config.Cookie.MaxAge = TimeSpan.FromMinutes(30);
    Config.SlidingExpiration = true;
    Config.AccessDeniedPath = "/Errors/403";
});

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
});
builder.Services.AddScoped<RewardService>();

builder.Services.AddScoped<PostService>();


builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<RequestService>();

builder.Services.AddTransient<EmailSender>();
builder.Services.Configure<EmailOptions>(builder.Configuration);

builder.Services.AddScoped<CategoryService>();

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

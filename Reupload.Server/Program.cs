using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data;
using Reupload.Server.Models;
using Reupload.Server.Options;
using System.IdentityModel.Tokens.Jwt;
using Hangfire;
using Hangfire.SqlServer;
using Reupload.Server.AzureStorage.Blobs;
using Reupload.Server.Data.Repositories;
using Reupload.Server.Data.Repositories.Contracts;
using Reupload.Server.Middlewares;
using Reupload.Server.Services;
using Reupload.Server.Services.Contracts;

// Services

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<
        ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

builder.Services
    .AddAuthentication()
    .AddIdentityServerJwt()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Facebook:AppId"];
        options.AppSecret = builder.Configuration["Facebook:AppSecret"];

        options.Events.OnRemoteFailure = (context) =>
        {
            context.Response.Redirect("/");
            context.HandleResponse();

            return Task.CompletedTask;
        };
    });

builder.Services.AddHangfire(options => options
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"),
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

builder.Services
    .AddControllersWithViews();

builder.Services
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection(AdminOptions.SectionName));
builder.Services.Configure<AzureStorageOptions>(builder.Configuration.GetSection(AzureStorageOptions.SectionName));

builder.Services.AddScoped<ISeeder, Seeder>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBlobStorageFactory, BlobStorageFactory>();

builder.Services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IUserActionService, UserActionService>();

// Middleware

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ISeeder seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();

    await seeder.SeedAdminAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CurrentUserMiddleware>();

app.MapRazorPages();
app.MapControllers();
app.MapHangfireDashboard();
app.MapFallbackToFile("index.html");

app.Run();
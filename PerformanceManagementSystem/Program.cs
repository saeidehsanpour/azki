using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PerformanceManagementSystem;
using PerformanceManagementSystem.Data;
using PerformanceManagementSystem.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
{
    options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.LogoutPath = new PathString("/Identity/Account/LogOut");
    options.LoginPath = new PathString("/Identity/Account/Login");
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthorization(options =>
{
    // options.FallbackPolicy = new AuthorizationPolicyBuilder()
    //     .RequireAuthenticatedUser()
    //     .Build();
    options.AddPolicy(Tools.Roles.President, policy =>
    {
        policy.RequireAssertion(context =>
            context.User.IsInRole(Tools.Roles.President));
    });
    options.AddPolicy(Tools.Roles.Administrator, policy =>
    {
        policy.RequireAssertion(context =>
            context.User.IsInRole(Tools.Roles.President) || context.User.IsInRole(Tools.Roles.Administrator));
    });
});
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Admin", "/Competencies", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("Admin", "/CompetencyCategories", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("Admin", "/CompetencyLevels", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("Admin", "/Organizations", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("Admin", "/Positions", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("Admin", "/Users", Tools.Roles.Administrator);
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/Exceptions");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/ManagerAssessments");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/ManagerEvaluations");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/ManagerReports");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/PeerAssessments");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/SelfAssessmentOveralls");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/SelfAssessments");
    options.Conventions.AuthorizeAreaFolder("PerformanceManagement", "/SelfReports");
    options.Conventions.AuthorizeAreaPage("PerformanceManagement", "/Index");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login");
});

builder.Services.AddAutoMapper(expression =>
    {
        expression.AddProfile<MappingProfile>();
    }
    , typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var hasAdmin = context.Users.Any(a => a.UserName == "admin@info.com");

    if (!hasAdmin)
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        var user = new AppUser
        {
            UserName = "admin@info.com",
            NormalizedUserName = "ADMIN@INFO.COM",
            Email = "admin@info.com",
            NormalizedEmail = "ADMIN@INFO.COM",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Firstname = "ادمین",
            Lastname = "خوبمون"
        };
        var createUser = userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();
        if (createUser.Succeeded)
        {
            var role = roleManager.CreateAsync(new AppRole
            {
                Name = Tools.Roles.Administrator,
                Id = Guid.NewGuid(),
                NormalizedName = Tools.Roles.Administrator.ToUpper()
            }).GetAwaiter().GetResult();
            var role1 = roleManager.CreateAsync(new AppRole
            {
                Name = Tools.Roles.President,
                Id = Guid.NewGuid(),
                NormalizedName = Tools.Roles.Administrator.ToUpper()
            }).GetAwaiter().GetResult();
            if (role.Succeeded)
            {
                userManager.AddToRoleAsync(user, Tools.Roles.Administrator).GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, Tools.Roles.President).GetAwaiter().GetResult();
                userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, $"{user.Firstname} {user.Lastname}")).GetAwaiter().GetResult();
            }
        }
    }
}
app.Run();

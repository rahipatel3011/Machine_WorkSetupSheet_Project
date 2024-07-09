using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Models;
using Machine_Setup_Worksheet.Repositories;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    string ReddisConnectionString = builder.Configuration.GetValue<string>("redis:ConnectionString")!;
    return ConnectionMultiplexer.Connect(ReddisConnectionString);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// enable identity in application
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();



// DI for all repository
builder.Services.AddScoped<IJawRepository, JawRepository>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IWorkSetupRepository, WorkSetupRepository>();
builder.Services.AddScoped<ISetupRepository, SetupRepository>();


// DI for all services
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IJawService, JawService>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IWorkSetupService, WorkSetupService>();
builder.Services.AddScoped<ISetupService, SetupService>();
builder.Services.AddScoped<IUserService, UserService>();





builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/status/403";
});


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

//ApplyMigration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    //app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



//void ApplyMigration()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        if (context != null)
//        {
//            context.Database.Migrate();
//        }
//    }
//}


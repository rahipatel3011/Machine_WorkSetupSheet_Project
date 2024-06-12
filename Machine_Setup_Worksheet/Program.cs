using Machine_Setup_Worksheet.Data;
using Machine_Setup_Worksheet.Repositories;
using Machine_Setup_Worksheet.Repositories.IRepository;
using Machine_Setup_Worksheet.Services;
using Machine_Setup_Worksheet.Services.IServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// DI for all repository
builder.Services.AddScoped<IJawRepository, JawRepository>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IWorkSetupRepository, WorkSetupRepository>();
builder.Services.AddScoped<ISetupRepository, SetupRepository>();


// DI for all services
builder.Services.AddScoped<IJawService, JawService>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IWorkSetupService, WorkSetupService>();
builder.Services.AddScoped<ISetupService, SetupService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

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

app.MapControllers();

app.Run();


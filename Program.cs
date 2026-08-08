using PersonRegistrationMvc.Data;
using PersonRegistrationMvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseRequestLocalization();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAndAdminAsync(scope.ServiceProvider, builder.Configuration);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Country",
    pattern: "{area:exists}/{controller=LOC_Country}/{action=LOC_CountryList}/{id?}");

app.MapControllerRoute(
    name: "State",
    pattern: "{area:exists}/{controller=LOC_State}/{action=LOC_CountryList}/{id?}");

app.MapControllerRoute(
    name: "City",
    pattern: "{area:exists}/{controller=LOC_City}/{action=LOC_CityList}/{id?}");

app.MapControllerRoute(
    name: "Branch",
    pattern: "{area:exists}/{controller=MST_Branch}/{action=MST_BranchList}/{id?}");

app.MapControllerRoute(
    name: "Student",
    pattern: "{area:exists}/{controller=MST_Student}/{action=MST_StudentList}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

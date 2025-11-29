using U5ProgWebPartituras.Models.Entities;
using U5ProgWebPartituras.Repositories;
using U5ProgWebPartituras.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

//Registros
builder.Services.AddDbContext<PartiturasContext>();
builder.Services.AddScoped<GenerosService>();
builder.Services.AddScoped<PartituraService>();
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));

//Nunca registres un registro después de build
var app = builder.Build();

//app.UseRouting();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapDefaultControllerRoute();

app.UseStaticFiles();

app.Run();




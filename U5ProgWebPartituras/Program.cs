using U5ProgWebPartituras.Models.Entities;
using U5ProgWebPartituras.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

//Registros
builder.Services.AddDbContext<PartiturasContext>();
//builder.Services.AddScoped<Service>();
//builder.Services.AddScoped<Service>();
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));

//Nunca registres un registro después de build
var app = builder.Build();

//app.UseRouting();

app.MapControllerRoute(
    name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapDefaultControllerRoute();

app.UseStaticFiles();

app.Run();




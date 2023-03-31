using PointOfSaleWeb.Repository;
using PointOfSaleWeb.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<InventoryDbContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration.GetSection("Kestrel:Endpoints:Https:Url");
var url = config.Value;

app.Urls.Add(url!);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();


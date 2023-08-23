using AspNetCore6WebAPI;
using AspNetCore6WebAPI.DbContexts;
using AspNetCore6WebAPI.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.
// Header => Accept : application/xml => return xml format , application/json or default return json

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The contents of the download file to be correct
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddTransient<IMailService,LocalMailService>();
#else
builder.Services.AddTransient<IMailService,CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityInfoContext>(
   dbContextOptions => dbContextOptions.UseSqlite("Data Source=CityInfo.db"));

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<CityInfoContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


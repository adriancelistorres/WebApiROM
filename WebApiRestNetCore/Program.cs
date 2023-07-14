using WebApiRestNetCore.GoogleSheetsAPI;
using WebApiRestNetCore.Services.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
   

    //options.AddDefaultPolicy(
    //policy =>
    //{
    //    //policy.WithOrigins("http://localhost/", "https://localhost/")
    //    //                                        .AllowAnyHeader()
    //    //                                        .AllowAnyMethod();


    //    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");

    //});
    options.AddDefaultPolicy(
policy =>
{
    policy.WithOrigins("http://localhost/", "https://localhost/","http://localhost:58893", "https://biweb.grupotawa.com", " https://biweb.grupotawa.com:443/","http://biweb.grupotawa.com/",  "http://biweb.grupotawa.com:443/")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
});

    //options.AddPolicy(name:_MyCors,builder=>

    //  {
    //      builder.SetIsOriginAllowed(origin=>new Uri(origin).Host=="http://localhost/")

    //  });

});

// Add services to the container.
builder.Services.AddHostedService<MyBackgroundService>();
builder.Services.AddHttpClient<MyBackgroundService>();
builder.Services.AddTransient<DataAcecss>(); // Agrega la dependencia del servicio DataAccess


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiRestNetCore.Authorization;
using WebApiRestNetCore.GoogleSheetsAPI;
using WebApiRestNetCore.Services.DataAccess;
using Microsoft.OpenApi.Models;


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
    policy.WithOrigins("http://localhost/", "https://localhost/","http://localhost:58893", "https://biweb.grupotawa.com", " https://biweb.grupotawa.com:443/","http://biweb.grupotawa.com/",  "http://biweb.grupotawa.com:443/", "http://localhost:4200","http://localhost:7117")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
});

    //options.AddPolicy(name:_MyCors,builder=>

    //  {
    //      builder.SetIsOriginAllowed(origin=>new Uri(origin).Host=="http://localhost/")

    //  });

});

// Add services to the container.

//adrian celis
builder.Services.AddHostedService<MyBackgroundService>();
builder.Services.AddHttpClient<MyBackgroundService>();
builder.Services.AddTransient<DataAcecss>(); // Agrega la dependencia del servicio DataAccess
builder.Services.AddScoped<TokenAuthorizationFilter>();

//


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Software Lion", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["JWT:Issuer"],
        //ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

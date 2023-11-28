using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MultiMarketing.Context;
using MultiMarketing.Model.Settings;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
IHostEnvironment environment = builder.Environment;

builder.Configuration.Sources.Clear();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Bunu Aşagıdaki yer ile degiştirdik başlangiç seviyesi olarak düşünebilirsin.
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", true, true);

ConfigurationSettingsSimple configurationSettingsSimple = new();
builder.Configuration.GetSection("Settings").Bind(configurationSettingsSimple);

var efconnector = builder.Configuration.GetConnectionString("DefaultConnection");

//IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
//var efconnector = config["ConnectionStrings:DefaultConnection"];


builder.Services.AddDbContext<MarketingDBContext>(options => options.UseSqlServer(efconnector));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mehmet'S Bolgesi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Dogrulama kodu gerekli",
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
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration["JwtSecurityToken:Audience"],
        ValidIssuer = builder.Configuration["JwtSecurityToken:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityToken:Key"])),
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.Configure<JwtSecurityTokenSettings>(builder.Configuration.GetSection("JwtSecurityToken"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var resp = "<html> <b> Bu izin size verilmemiştir. Lütfen yetkili ile iletişime geçiniz. </b></html>";
app.UseStatusCodePages(Text.Html, resp);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

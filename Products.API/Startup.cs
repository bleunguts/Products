using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Products.API;
using Products.API.Services;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options => 
        {
            options.JsonSerializerOptions.Converters.Add(new ColorJsonConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var jwtScheme = new OpenApiSecurityScheme()
            {
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                Description = "Put JWT Bearer token in textbox without **'Bearer'**",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition("Bearer", jwtScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { jwtScheme, Array.Empty<string>()}
            });
            options.MapType(typeof(Color), () => new OpenApiSchema
            {
                Type="string",
                Example = new OpenApiString(Color.Gold.Name)
            });
        });
        services.AddHealthChecks();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = configuration["jwt:issuer"],
                ValidAudience = configuration["jwt:audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"])),
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,                             
            };
        });
        
        services.Configure<ApplicationSettings>(options =>
        {
            options.JwtKey = configuration["jwt:key"];
            options.JwtIssuer = configuration["jwt:issuer"];
            options.JwtAudience = configuration["jwt:audience"];
            options.DataStoreJsonFileName = configuration["backingDataStore:jsonFileName"];
        });   

        services.TryAddScoped<IMapper, Mapper>();
        services.TryAddScoped<IProductRepository, ProductRepository>();
        services.TryAddSingleton<IProductJsonDataStoreService, ProductJsonDataStoreService>();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();            
        }

        app.UseHttpsRedirection();        

        app.UseAuthentication();             

        app.UseAuthorization();
       
        app.MapControllers();

        app.MapHealthChecks("/healthCheck");        
    }
}

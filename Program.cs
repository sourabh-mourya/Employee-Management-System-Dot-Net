using System.Text;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Helper;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("dbcs");

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// services register
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddScoped<TokenHelper>();

// JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))
        };

        options.Events = new JwtBearerEvents
        {
            // 401: token nahi diya, galat hai ya expire ho gaya
            //then ye wala error show hoga
            OnChallenge = async context =>
            {
                context.HandleResponse();//default empty repsonse band kro 

                // 401: token nahi diya, galat hai ya expire ho gaya
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    status = 401,
                    message = "You are not logged in. Please login and send a valid token."

                });
            },
            // 403: login hai, par role allowed nahi (jaise Employee ne Admin API call ki)

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    status = 403,
                    message = "You do not have permission to access this resource. Admin access required."
                });

            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();   // pehle authentication
app.UseAuthorization();    // phir authorization (order important hai)

app.MapControllers();
app.Run();
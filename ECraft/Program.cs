using ECraft.Data;
using ECraft.Models;
using ECraft.Models.Identity;
using ECraft.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using ECraft.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using ECraft;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


#region Identity Configuraion

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

    JwtSettings? jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    if (jwtSettings == null)
    {
        throw new Exception("JwtSettings Config Error");
    }

builder.Services.AddSingleton(jwtSettings);

    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey=true,
        ValidateLifetime = jwtSettings.ValidateLifeTime,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ValidAudience=jwtSettings.Audience,
        ValidIssuer=jwtSettings.Issuer,
        ValidateIssuer=true,
        ClockSkew=TimeSpan.FromSeconds(0)
    };

    builder.Services.AddSingleton(tokenValidationParameters);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(jwtOptions =>
    {
        jwtOptions.TokenValidationParameters = tokenValidationParameters;
    });

#endregion

builder.Services.RegisterServices();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

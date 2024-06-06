using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReposatoryPatternWithUOW.Core.Interfaces;
using ReposatoryPatternWithUOW.Core.OptionsPatternClasses;
using ReposatoryPatternWithUOW.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using ReposatoryPatternWithUOW.EF.Reposatories;
using ReposatoryPatternWithUOW.EF.Mapper;
using ReposatoryPatternWithUOW.EF.MailService;
using Microsoft.Extensions.DependencyInjection;

//using MailKit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var conStr=builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer(conStr).UseLazyLoadingProxies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IToken,TokenGenerator>();
builder.Services.AddScoped<ISenderService,MailService>();
builder.Services.AddScoped<Mapperly>();

builder.Services.Configure<TokenOptionsPattern>(builder.Configuration.GetSection("JWT"));
var JwtSettings = builder.Configuration.GetSection("JWT").Get<TokenOptionsPattern>();
builder.Services.AddSingleton(JwtSettings!);

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
var MailSettings = builder.Configuration.GetSection("MailSettings").Get<MailSettings>();
builder.Services.AddSingleton(MailSettings!);


builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opts =>
    {

        opts.RequireHttpsMetadata = true;
        opts.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = JwtSettings.Audience,
            ValidIssuer = JwtSettings.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
            RoleClaimType = ClaimTypes.Role,

        };
    });



    var app = builder.Build();
app.UseCors(c=>c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

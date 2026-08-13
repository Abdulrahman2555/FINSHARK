using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Interfaces;
using api.Repository;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using api.Service;
// using api.Interfaces;
// using api.Models;
// using api.Repository;
// using api.Service;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
  options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddScoped<ICommentRepository,CommentRepository>();
builder.Services.AddScoped<IStockRepository,StockRepository>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// add identity service
builder.Services.AddIdentity<AppUser,IdentityRole>(
  Options =>
  {
    Options.Password.RequireDigit=true;

    Options.Password.RequireLowercase=true;
    Options.Password.RequireUppercase=true;
    Options.Password.RequireNonAlphanumeric=true;
    Options.Password.RequiredLength=12;
  }
)
.AddEntityFrameworkStores<ApplicationDBContext>();

// Add authentication service
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme= 
  options.DefaultChallengeScheme=
  options.DefaultForbidScheme=
  options.DefaultScheme=
  options.DefaultScheme=
  options.DefaultSignInScheme =
  options.DefaultSignOutScheme =JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(Options =>
{
  Options.TokenValidationParameters=new TokenValidationParameters
  {
    ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
  };

});





var app = builder.Build();


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

app.Run();

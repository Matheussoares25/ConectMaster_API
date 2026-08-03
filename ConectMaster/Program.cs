using Microsoft.EntityFrameworkCore;
using ConectMaster.Bancodedados;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    try
    {
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)
        );
    } 
    catch (Exception ex)
    {
        throw new InvalidOperationException("Ocorreu um erro ao configurar o banco de dados: ");
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Authentication / JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CHANGE_THIS_SECRET_KEY_1234567890";
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})



.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
        ValidIssuer = jwtIssuer,
        ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
        ValidAudience = jwtAudience,
        ValidateLifetime = true
    };
});



builder.Services.AddAuthorization();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("ReactPolicy");
//swagger
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

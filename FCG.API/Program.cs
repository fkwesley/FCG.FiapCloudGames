using FCG.Application.Interfaces;
using FCG.Application.Services;
using FCG.Domain.Repositories;
using FCG.Infrastructure.Context;
using FCG.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
var jwtKey = configuration["Jwt:Key"];
var jwtIssuer = configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,  // Verifica se o issuer (emissor) bate
            ValidateAudience = false, // Neste exemplo não validamos a audiência
            ValidateLifetime = true,  // Valida se o token ainda não expirou
            ValidateIssuerSigningKey = true, // Valida a assinatura do token
            ValidIssuer = jwtIssuer,  // Define o emissor esperado
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Chave usada para verificar assinatura
        };

        //Configurando mensagens amigáveis
        options.Events = new JwtBearerEvents
        {
            // Quando não autenticado (token inválido ou ausente)
            OnChallenge = context =>
            {
                context.HandleResponse(); // para evitar o retorno padrão

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    message = "Invalid Token, not Authenticated."
                });

                return context.Response.WriteAsync(result);
            },

            // Quando autenticado, mas sem permissão (ex: não tem role)
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    message = "Access Denied! Only Admin users can run this operation."
                });

                return context.Response.WriteAsync(result);
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FCG.FiapCloudGames.API", Version = "v1" });

    // Define o esquema de segurança JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer", // Nome do esquema (obrigatório)
        BearerFormat = "JWT", // Apenas informativo
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    // Define a aplicação do esquema de segurança aos endpoints protegidos
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>() // Sem escopos específicos
        }
    });
    // Configuração para habilitar summaries
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion

// Dependency Injection for Application Layer
builder.Services.AddSingleton<IAuthService>(new AuthService(jwtKey, jwtIssuer));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Register the DbContext with dependency injection
builder.Services.AddDbContext<FiapCloudGamesDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("FiapCloudGamesDbConnection"));
}, ServiceLifetime.Scoped);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware que valida autenticação JWT em cada requisição
app.UseAuthentication();

// Middleware que avalia autorizações (ex: [Authorize])
app.UseAuthorization();

app.MapControllers();

app.Run();

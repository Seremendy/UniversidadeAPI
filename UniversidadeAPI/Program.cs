using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.Data;
using System.Text;
using UniversidadeAPI.Repositories;
using UniversidadeAPI.Repositories.Interfaces;
using UniversidadeAPI.Services;

namespace UniversidadeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- CONFIGURAÇÃO DA CONEXÃO DB ---
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");


            // Adiciona o suporte aos Controladores
            builder.Services.AddControllers();

            builder.Services.AddScoped<IDbConnection>(provider =>
            {
                return new MySqlConnection(connectionString);
            });


            builder.Services.AddSingleton<ITokenService, TokenService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
            builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
            builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
            builder.Services.AddScoped<ICursoRepository, CursoRepository>();
            builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();
            builder.Services.AddScoped<ISalaDeAulaRepository, SalaDeAulaRepository>();
            builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
            builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
            builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            builder.Services.AddScoped<INotaRepository, NotaRepository>();
            builder.Services.AddScoped<IPrerequisitoRepository, PrerequisitoRepository>();
            builder.Services.AddScoped<IGradeCurricularRepository, GradeCurricularRepository>();


            // --- CONFIGURAÇÃO DO SWAGGER/OPENAPI ---
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "UniversidadeAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {seu token}'",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Configuração do JWT
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("Chave JWT não configurada.");

            // Adiciona o serviço de Autenticação
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            // Adiciona o serviço de Autorização
            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirAngular", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // A URL do seu front Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // --- CONFIGURAÇÃO DO PIPELINE HTTP ---

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("PermitirAngular");
            app.UseAuthentication();
            app.UseAuthorization();

            // Configura o roteamento para os Controladores
            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;
            var connectionString = configuration.GetConnectionString("NpgConnection");
            builder.Services.AddDbContext<DbContexts.NpgDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidIssuer = Services.Authentication.AuthenticationOptions.GetIssuer(configuration),

                       ValidateAudience = true,
                       ValidAudience = Services.Authentication.AuthenticationOptions.GetAudience(configuration),

                       ValidateLifetime = true,

                       IssuerSigningKey = Services.Authentication.AuthenticationOptions.GetSymmetricSecurityKey(configuration),
                       ValidateIssuerSigningKey = true
                   };
               });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<DbContexts.NpgDbContext>();

            builder.Services.AddTransient<Services.AccountService>();
            builder.Services.AddTransient<Services.SecurityOptions.SecurityService>();

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
        }
    }
}

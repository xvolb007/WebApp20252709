using Application.Interaces;
using Application.Services;
using Infrastructure.Converters;
using Infrastructure.Messaging;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WebApp20252709
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<CalculationService>();

            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));
            builder.Services.AddSingleton<RabbitMqInitializer>();
            builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
            builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
            builder.Services.AddHostedService<RabbitMqHostedService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("Database");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
            );

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DecimalFullPrecisionConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await db.Database.MigrateAsync();

                var initializer = scope.ServiceProvider.GetRequiredService<RabbitMqInitializer>();
                initializer.InitializeAsync().GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using System.Text;
using System.Text.Json.Serialization;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using user_service.Authentication;
using user_service.Consumer;
using user_service.Data;
using user_service.Model;
using user_service.Repository;
using user_service.Repository.Interfaces;
using user_service.Services;
using Prometheus;
using Microsoft.AspNetCore.HttpOverrides;

namespace user_service
{
    public class Program
    {

        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers().AddJsonOptions(options =>
             options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("role", "ADMIN"));
                options.AddPolicy("User", policy => policy.RequireClaim("role", "USER"));
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
                });
            });
            if (FirebaseApp.DefaultInstance == null)
            {
                builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("./firebase-config.json")
                }));
            }

            //firebase
            // string firebaseConfigJson = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");
            // builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
            // {
            //     Credential = GoogleCredential.FromJson(firebaseConfigJson)
            // }));

            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING_USER");
            var databaseName = Environment.GetEnvironmentVariable("DATABASENAME_USER");
            var collectionName = Environment.GetEnvironmentVariable("COLLECTIONNAME_USER");
            builder.Services.Configure<UserDatabaseConfiguration>(options =>
            {
                options.ConnectionString = connectionString;
                options.DatabaseName = databaseName;
                options.UsersCollectionName = collectionName;
            });

            // builder.Services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase"));
            // });

            // builder.Services.AddDbContext<AppDbContext>(options =>
            // {
            //     options.UseNpgsql("Server=user_postgres_db;Port=5432;Database=user_db;Username=root;Password=root",
            //         x => { x.MigrationsAssembly("User service"); });
            // });

            /*builder.Services.Configure<UserDatabaseConfiguration>(
                builder.Configuration.GetSection("ConfigurationDatabase")
            );*/

            builder.Services.AddHostedService<UserConsumer>();
            // builder.Services.AddSingleton<UserConsumer>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<ILambdaService, LambdaService>(); // Register the Lambda service
            builder.Services.AddTransient<IUserService, UserService>(); // Ensure UserService is registered





            // var factory = new ConnectionFactory() { HostName = "localhost" };
            // var connection = factory.CreateConnection();
            // using var _channel = connection.CreateModel();

            // _channel.QueueDeclare(queue: "users");
            // var Consumer = new EventingBasicConsumer(_channel);
            // Consumer.Received += (model, ea) =>
            // {
            //     var body = ea.Body.ToArray();
            //     var message = Encoding.UTF8.GetString(body);
            //     Console.WriteLine(message);
            // };
            // _channel.BasicConsume(queue: "users", autoAck: true, consumer: Consumer);
            // Console.ReadLine();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMetricServer();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });
            app.MapControllers();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(builder =>
            builder
          .WithOrigins("http://localhost:5173")
          .AllowAnyMethod()
          .AllowAnyHeader());

            app.Run();


        }
    }
}

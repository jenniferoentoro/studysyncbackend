using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using source_service.Repository;
using source_service.Repository.Interface;
using source_service.Service;
using source_service.Service.Interface;
using source_service.Authentication;
using source_service.Data;
using source_service.Model;
using source_service.Consumer;
using Prometheus;

using Azure.Storage.Blobs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Source Service", Version = "v1" });
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


builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase-config.json")
}));



var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING_SOURCE");
var databaseNameSource = Environment.GetEnvironmentVariable("DATABASE_NAME_SOURCE");
var collectionName = Environment.GetEnvironmentVariable("COLLECTION_NAME_CATEGORY");
var collectionNameSource = Environment.GetEnvironmentVariable("COLLECTION_NAME_SOURCE");
var FileStorageCollectionName = Environment.GetEnvironmentVariable("COLLECTION_NAME_FILESTORAGE");
builder.Services.Configure<DatabaseConfiguration>(options =>
{
    options.ConnectionString = connectionString;
    options.DatabaseNameSource = databaseNameSource;
    options.CategoriesCollectionName = collectionName;
    options.SourcesCollectionName = collectionNameSource;
    options.FileStorageCollectionName = FileStorageCollectionName;
});


builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient("withconnectionSting");
});
builder.Services.AddHostedService<SourceConsumer>();

builder.Services.AddScoped<ICacheService, CacheServise>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ISourceRepository, SourceRepository>();
builder.Services.AddScoped<ISourceService, SourceService>();


builder.Services.AddScoped<IFileStorageRepository, FileStorageRepository>();

builder.Services.AddScoped<IFileStorageService, FileStorageService>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();
app.UseMetricServer();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpMetrics(options =>
            {
                options.AddCustomLabel("host", context => context.Request.Host.Host);
            });
app.UseHttpsRedirection();

app.MapControllers();


app.UseCors(builder =>
           builder
         .WithOrigins("http://localhost:5173")
         .AllowAnyMethod()
         .AllowAnyHeader());
app.Run();

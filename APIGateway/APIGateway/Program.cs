using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

using APIGateway.Authentication;
using Microsoft.AspNetCore.Authentication;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton(FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./firebase-config.json")
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (o) => { });


builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
         builder =>
         {
             builder.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
         });

});
var app = builder.Build();

app.UseCors("AllowSpecificOrigins");



await app.UseOcelot();

app.Run();

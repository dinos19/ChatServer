using ChatServer.Hubs;
using ChatServer.Infrastructure;
using Microsoft.AspNetCore.Authentication.Certificate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddAuthentication(
    CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowSpecificOrigin",
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:7075")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
});
builder.Services.AddSingleton<SharedDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");
app.UseAuthentication();
app.UseCors("AllowSpecificOrigin"); // Apply the CORS policy

app.Run();
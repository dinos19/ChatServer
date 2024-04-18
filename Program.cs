using ChatServer.Hubs;
using ChatServer.Infrastructure;
using ChatServer.Infrastructure.Repositories;
using ChatServer.Infrastructure.Repositories.BaseAbstractions;
using ChatServer.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5001); // Change the port as necessary
});

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
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .DisallowCredentials(); // Ensure credentials are not allowed
        });
});
builder.Services.AddSingleton<SharedDB>();
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<AccountHandler>();
builder.Services.AddTransient<AccountRepository>();
builder.Services.AddTransient<ChatMessageRepository>();
builder.Services.AddTransient<UserConnectionRepository>();
builder.Services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();

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
app.MapHub<SyncHub>("/sync");
app.UseAuthentication();
app.UseCors("AllowAllOrigins"); // Apply the CORS policy

app.Run();
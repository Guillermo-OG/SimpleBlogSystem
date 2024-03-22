using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleBlogSystem.Data;
using SimpleBlogSystem.Interfaces;
using SimpleBlogSystem.Models;
using SimpleBlogSystem.Services; // Asegúrate de importar el namespace correcto para UserService

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<BlogDbContext>();
builder.Services.AddControllersWithViews();

// Registra PostService para a injeção de dependência
builder.Services.AddScoped<IPostService, PostService>();

// Aqui se registra o UserService
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddSingleton<WebSocketNotificationService>();

var app = builder.Build();

app.UseWebSockets();

// Map WebSocket requests to the WebSocketNotificationService
app.Use(async (context, next) =>
{
    if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/notifications")
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("WebSocket request for /notifications received.");

        var wsService = app.Services.GetRequiredService<WebSocketNotificationService>();
        if (wsService != null)
        {
            await wsService.InitializeWebSocketAsync(context, next);
            logger.LogInformation("WebSocket connection established.");
        }
        else
        {
            context.Response.StatusCode = 500;
            logger.LogError("WebSocketNotificationService not available.");
        }
    }
    else
    {
        await next.Invoke();
    }
});
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

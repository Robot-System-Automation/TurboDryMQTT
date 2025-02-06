using TurboDryMQTT.Hubs;
using TurboDryMQTT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<MqttService>();
builder.Services.AddSignalR();

// Add CORS services
builder.Services.AddCors(options =>
{
    //options.AddPolicy("AllowSpecificOrigin", policy =>
    //{
    //    policy.WithOrigins("http://localhost:5000")
    //          .AllowAnyHeader()
    //          .AllowAnyMethod()
    //          .AllowCredentials();
    //});
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
        "http://localhost:5159",
        "http://172.31.10.136:5159",
        "http://172.31.10.100")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseDefaultFiles();

// Serve static files
app.UseStaticFiles();

app.UseFileServer();

// Enable routing
app.UseRouting();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Map SignalR hub
app.MapHub<MqttHub>("/mqttHub");

// Default route
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("MQTT Web App is running...");
});

app.MapFallbackToFile("index.html");

// Run the application
//app.Run();
app.Run("http://172.31.10.136:5159");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<FileService>();
builder.Services.AddScoped<OpenRouterService>();
builder.Services.AddScoped<AgentService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("all", p =>
    {
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("all");
app.MapControllers();
app.Run();
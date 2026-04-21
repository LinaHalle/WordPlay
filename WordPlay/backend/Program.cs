using Brainfart;

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<CategoryService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseCors("AllowFrontend");

app.MapGameEndpoints(app.Services.GetRequiredService<GameService>(), app.Services.GetRequiredService<CategoryService>());

app.MapFallbackToFile("index.html");

app.Run();

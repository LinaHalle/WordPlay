using Brainfart;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// CORS (frontend access)
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

// Swagger (OpenAPI för .NET 8)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors("AllowFrontend");

// Swagger UI (bara i dev)
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// REMOVE or disable for Render
// app.UseHttpsRedirection();

app.MapGameEndpoints();

// Makes it so freontend shwos and handles routes
app.MapFallbackToFile("index.html");

app.Run();
using Brainfart;

var builder = WebApplication.CreateBuilder(args);

//CORS (frontend access)
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
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGameEndpoints();

app.Run();


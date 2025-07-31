using FoodPlanner.API.Middlewares;
using FoodPlanner.Application.Configurations;
using FoodPlanner.Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

// Layers
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
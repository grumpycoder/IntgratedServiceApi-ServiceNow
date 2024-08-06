using IntegratedServicesApi.Nuvolo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/servicecalls", (ServiceCall serviceCall) =>
{
    if(serviceCall == null)
    {
        return Results.BadRequest("Service call is required");
    }
    
    // Save the service call to the database
    return Results.Ok(new {Message = "Service call received.", serviceCall});
}).WithName("ServiceCall")
.WithOpenApi()
.WithDescription("Create a new service call from ServiceNow."); 


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
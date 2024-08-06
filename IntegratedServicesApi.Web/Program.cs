using IntegratedServicesApi.Nuvolo.Models;
using Okta.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
}).AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = "https://dev-91388862.okta.com/oauth2/default",
    Audience = "api://default"
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ServiceCallPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scp", "workorder.read");
    });
});

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-91388862.okta.com/oauth2/default"; // Replace with your Okta domain
        options.Audience = "api://default"; // Replace with your Okta API audience
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/servicecalls", (ServiceCall serviceCall) =>
    {
        // Save the service call to the database
        return Results.Ok(new { Message = "Service call received.", serviceCall });
    }).WithName("ServiceCall")
    .WithOpenApi()
    .WithDescription("Create a new service call from ServiceNow.")
    .RequireAuthorization("ServiceCallPolicy");


app.Run();
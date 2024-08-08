using IntegratedServicesApi.Nuvolo.Models;
using IntegratedServicesApi.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Okta.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
    
})
    .AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = "https://dev-91388862.okta.com/oauth2/default",
    Audience = "https://localhost-workorders", 
});*/

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://dev-91388862.okta.com/oauth2/default"; // Replace with your Okta domain
        options.Audience = "https://localhost-workorders"; // Replace with your Okta API audience
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            /*ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true*/
        };
    });

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "https://dev-udeaeqkktb02bhqw.us.auth0.com"; // Replace with your Okta domain
//         options.Audience = "api-sandbox"; // Replace with your Okta API audience
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             /*ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true*/
//         };
//     });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ServiceCallPolicy", policy =>
    { 
        policy.RequireAuthenticatedUser();
        //policy.RequireClaim("read.workorders", "true");
        // policy.RequireClaim("scope", "read.workorders write.workorders");
        // policy.RequireClaim("scp", "workorder.read");
        policy.RequireScopes("workorder.read");
    });
});



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

app.MapPost("/api/servicecalls", async (HttpContext ctx) =>
    {
        // Save the service call to the database
        string token = await ctx.GetTokenAsync("access_token");
        return Results.Ok(new { Message = "Service call received."});
    }).WithName("ServiceCall")
    //.WithOpenApi()
    .WithDescription("Create a new service call from ServiceNow.")
    // .RequireAuthorization("ServiceCallPolicy")
    
    ;


app.Run();
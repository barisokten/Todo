using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TodoApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// API 5000 portundan dinler
builder.WebHost.UseUrls("https://localhost:5001");

// DbContext
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add custom authentication scheme
builder.Services.AddAuthentication("CustomAuth")
    .AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>("CustomAuth", options => { });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue",
        policy => policy
            .WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:5001/swagger", "http://localhost:5001/swagger")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
    db.Database.Migrate();
}

app.UseCors("AllowVue");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Todo API çalışıyor!");

app.Run();

// Custom Authentication Handler Classes
public class CustomAuthenticationOptions : AuthenticationSchemeOptions
{
}

public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
{
    public CustomAuthenticationHandler(
        IOptionsMonitor<CustomAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        Console.WriteLine("🔐 Custom Auth - RAW HEADER => '" + authorizationHeader + "'");

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("❌ Custom Auth - No Bearer token found");
            return Task.FromResult(AuthenticateResult.Fail("No Bearer token found"));
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
        Console.WriteLine("🔐 Custom Auth - Extracted Token => '" + token + "'");

        var parts = token.Split('.');
        if (parts.Length != 3)
        {
            Console.WriteLine("❌ Custom Auth - Invalid JWT structure");
            return Task.FromResult(AuthenticateResult.Fail("Invalid JWT structure"));
        }

        try
        {
            // Decode the payload
            var payload = parts[1];
            var padding = new string('=', (4 - payload.Length % 4) % 4);
            payload = payload.Replace('-', '+').Replace('_', '/') + padding;
            var jsonBytes = Convert.FromBase64String(payload);
            var json = System.Text.Encoding.UTF8.GetString(jsonBytes);
            Console.WriteLine("🔐 Custom Auth - JWT Payload => " + json);

            var claims = new List<Claim>();
            using var jsonDocument = System.Text.Json.JsonDocument.Parse(json);
            var root = jsonDocument.RootElement;

            if (root.TryGetProperty("user_id", out var userIdElement))
            {
                claims.Add(new Claim("sub", userIdElement.GetString()));
            }

            if (root.TryGetProperty("email", out var emailElement))
            {
                claims.Add(new Claim("email", emailElement.GetString()));
            }

            if (root.TryGetProperty("name", out var nameElement))
            {
                claims.Add(new Claim("name", nameElement.GetString()));
            }
        
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Console.WriteLine("✅ Custom Auth - Authentication successful");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Custom Auth - Error processing token: " + ex.Message);
            return Task.FromResult(AuthenticateResult.Fail("Error processing token"));
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = "Bearer";
        return Task.CompletedTask;
    }
}
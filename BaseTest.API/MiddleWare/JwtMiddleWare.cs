using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using BaseTest.Common;
using BaseTest.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace BaseTest.API.Middlewave;

public class JwtMiddleWare : IMiddleware
{
    private readonly IConfiguration configuration;

    public JwtMiddleWare(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
        if (string.IsNullOrEmpty(token))
        {
            token = Convert.ToString(context.Request.Query["access_token"]);
        }

        if (token != null)
        {
            AttachUserToContext(context, token);
            await next(context);
            return;
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
        
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(configuration.GetSection(Constant.AppSettingKeys.SECRET_KEY).Value!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

            var role = jwtToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role);

            context.Items[ClaimTypes.Role] = role;

        }
        catch(Exception ex)
        {
            var errorResponse = new
            {
                Message = "Unauthorized: Invalid Roles or Permissions",
                Error = ex.Message
            };
            
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
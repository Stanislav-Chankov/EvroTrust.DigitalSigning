using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EvroTrust.DigitalSigning.WebApi.Extensions
{
    internal static class IdentityRegistrar
    {
        //internal static void AddAuthorizationPolicy(this IServiceCollection services)
        //{
        //    services.AddAuthorization(options =>
        //    {
        //        foreach (var roleAction in RolesConfig.RoleActions)
        //        {
        //            IEnumerable<string> allowedRoles = roleAction.Value
        //                .Where(roleEnableKvp => roleEnableKvp.Value)
        //                .Select(roleEnableKvp => roleEnableKvp.Key.ToString());

        //            options.AddPolicy(
        //                roleAction.Key.ToString(),
        //                builder => builder.RequireClaim(ClaimTypes.Role, allowedRoles));
        //        }
        //    });
        //}

        internal static void AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var jwtOptions = new JwtOptions
            {
                Key = builder.Configuration["Jwt:Key"],
                Issuer = builder.Configuration["Jwt:Issuer"],
                Audience = builder.Configuration["Jwt:Audience"]
            };

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;

                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = context =>
                    //    {
                    //        // Log.Information($"Authentication failed. Details: {context.Exception}.");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnTokenValidated = context =>
                    //    {
                    //        // Log.Information($"Token validated. Details: {context}");
                    //        return Task.CompletedTask;
                    //    },
                    //    OnMessageReceived = context =>
                    //    {
                    //        var accessToken = context.Request.Query["access_token"];

                    //        if (!string.IsNullOrWhiteSpace(accessToken))
                    //        {
                    //            context.Token = accessToken;
                    //        }

                    //        return Task.CompletedTask;
                    //    }
                    //};

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
                    };
                });
        }
    }
}

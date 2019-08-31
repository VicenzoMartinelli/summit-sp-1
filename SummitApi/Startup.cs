using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SummitApi.Context;
using SummitApi.Domain;
using SummitApi.Repository;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Text;

namespace SummitApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      AddSwagger(services);

      services.AddDbContext<ContextoSummit>();

      AddIdentityJwt(services);

      services.AddScoped<IUsuarioRepository, SqlSeverUsuarioRepository>();
    }

    private static void AddIdentityJwt(IServiceCollection services)
    {
      services
          .AddDefaultIdentity<Usuario>(options =>
          {
            options.Password.RequireDigit           = false;
            options.Password.RequireLowercase       = false;
            options.Password.RequireUppercase       = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength         = 1;
          })
          .AddEntityFrameworkStores<ContextoSummit>()
          .AddDefaultTokenProviders();

      services
        .AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
        .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey         = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SponteSummit2019")),
          ValidateAudience         = false,
          ValidateIssuer           = false,
          ValidateLifetime         = false
        };
      });
    }

    private void AddSwagger(IServiceCollection services)
    {
      services.AddSwaggerGen(s =>
      {
        s.SwaggerDoc("v1", new Info
        {
          Version = "v1",
          Title = "Summit API",
          Description = "Summit API",
          Contact = new Contact
          {
            Name = "Summit 2019",
            Email = "martinellivicenzo@gmail.com"
          }
        });

        Dictionary<string, IEnumerable<string>> security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } };

        s.AddSecurityDefinition(
            "Bearer",
            new ApiKeyScheme
            {
              In = "header",
              Description = "Copiar 'Bearer ' + token'",
              Name = "Authorization",
              Type = "apiKey"
            });

        s.AddSecurityRequirement(security);

        s.OrderActionsBy(x => x.ActionDescriptor.DisplayName);
      });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseDeveloperExceptionPage();

      app.UseAuthentication();

      app.UseMvc();

      app.UseSwagger();

      app.UseSwaggerUI(s =>
      {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Summit API");
      });
    }
  }
}

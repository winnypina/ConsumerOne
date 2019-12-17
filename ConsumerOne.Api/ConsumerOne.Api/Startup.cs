using System;
using System.Net;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using ConsumerOne.Api.Auth;
using ConsumerOne.Api.Data;
using ConsumerOne.Api.Extensions;
using ConsumerOne.Api.Helpers;
using ConsumerOne.Api.Models;
using ConsumerOne.Api.Services;

namespace ConsumerOne.Api
{
    public class Startup
    {
        private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
        private const string _defaultTokenProviderName = "Default";


        private readonly SymmetricSecurityKey
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b =>
                    {
                        b.MigrationsAssembly("ConsumerOne.Api");
                        b.UseNetTopologySuite();
                    }));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            // Register the ConfigurationBuilder instance of FacebookAuthSettings
            services.Configure<FacebookAuthSettings>(Configuration.GetSection(nameof(FacebookAuthSettings)));

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<DefaultDataProtectorTokenProviderOptions>(options =>
                {
                    options.TokenLifespan = TimeSpan.FromDays(365);
                });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };


            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IEmailSender, EmailSender>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser",
                    policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol,
                        Constants.Strings.JwtClaims.ApiAccess));
            });
            services.AddCors();
            // add identity
            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                o.ClaimsIdentity.UserIdClaimType = Constants.Strings.JwtClaimIdentifiers.Id;
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 2;

            })
                .AddTokenProvider<DefaultDataProtectorTokenProvider<AppUser>>(_defaultTokenProviderName);
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.Configure<IISOptions>(options =>
            {

            });

            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<AppUser> userManager)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });
            app.UseCors(builder =>
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            SeedUsers(userManager);
            
        }

        public static async void SeedTranslations(ApplicationDbContext db)
        {
            var service = new BaseLanguageCreator(db);
            await service.Create();
        }

        public static void SeedUsers
            (UserManager<AppUser> userManager)
        {
            if (userManager.FindByEmailAsync
                    ("admin@consumer.one").Result == null)
            {
                var user = new AppUser();
                user.UserName = "admin@consumer.one";
                user.Email = "admin@consumer.one";
                user.BirthDate = new DateTime(1960, 1, 1);
                user.Type = UserType.Admin;
                var result = userManager.CreateAsync
                    (user, "adm@con123").Result;
            }
        }
    }


}
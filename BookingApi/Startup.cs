using System;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BookingApi.Data;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using BookingApi.Data.Repository.AirportRepo;
using BookingApi.Data.Repository.BookingRepo;
using BookingApi.Data.Repository.CustomerRepo;
using BookingApi.Data.Repository.DepartureRepo;
using BookingApi.Data.Repository.DestinationRepo;
using BookingApi.Data.Repository.FlightRepo;
using BookingApi.Data.Repository.PassengerRepo;
using BookingApi.Data.Repository.SeatRepo;
using BookingApi.Models;

namespace BookingApi
{
    public class Startup
    {
        private const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                //options.AddPolicy(name: MyAllowSpecificOrigins,
                //    builder =>
                //    {
                //        builder.WithOrigins("http://localhost:3000", "https://eagleairlines.netlify.app")
                //            .WithExposedHeaders("x-pagination");
                //    });

                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://localhost:5003")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "BookingAPI");
                });
            });

            services.AddDbContext<BookingContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("BookingApiConnection")));

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IAirportRepo, AirportRepo>();
            services.AddScoped<IDepartureRepo, DepartureRepo>();
            services.AddScoped<IDestinationRepo, DestinationRepo>();
            services.AddScoped<IFlightRepo, FlightRepo>();
            services.AddScoped<ISeatRepo, SeatRepo>();
            services.AddScoped<IBookingRepo, BookingRepo>();
            services.AddScoped<IPassengerRepo, PassengerRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Booking API",
                    Description = "The EagleAirlines booking API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Claude Christ",
                        Email = "christ.tchambila@gmail.com",
                        Url = new Uri("https://cchris.netlify.app"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }

                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Eagle Airlines Booking API");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(MyAllowSpecificOrigins);

            app.UseCors("default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                 .RequireAuthorization("ApiScope");
            });
        }
    }
}

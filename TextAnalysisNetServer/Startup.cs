using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace TextAnalysis
{
	public class Startup
	{
		public static IConfiguration staticConfiguration { get; set; }
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			staticConfiguration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<TextAnalysisDatabaseSettings>(Configuration.GetSection(nameof(TextAnalysisDatabaseSettings)));
			services.AddSingleton<ITextAnalysisDatabaseSettings>(sp => sp.GetRequiredService<IOptions<TextAnalysisDatabaseSettings>>().Value);

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
				builder =>
				{
					builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});
			});
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
			services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
			//services.AddAuthorization(options =>
			//{
			//	options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("admin"));
			//	options.AddPolicy("RequireRegistratedRole", policy => policy.RequireRole("admin", "vip", "registrated"));
			//});


			// configure strongly typed settings objects
			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);
			var textAnalysisSettingsSection = Configuration.GetSection("TextAnalysisDatabaseSettings");
			services.Configure<TextAnalysisDatabaseSettings>(textAnalysisSettingsSection);

			// configure jwt authentication
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});

			services.AddScoped<IUserRepository, UserManager>();
			services.AddScoped<IUserAnaliticsRepository, UserAnaliticsManager>();

			services.AddScoped<IFullAnalyticsRepository, FullAnalyticsManager>();

			services.AddScoped<ISynonimRepository, SynonimManager>();
			services.AddScoped<IAntonimRepository, AntonimManager>();

			services.AddScoped<IArchaismRepository, ArchaismManager>();
			services.AddScoped<IIrregularVerbsRepository, IrregularVerbsManager>();
			services.AddScoped<ISlangRepository, SlangManager>();
			services.AddScoped<IExpressionsRepository, ExpressionsManager>();

			services.AddScoped<ITemporalSingleWordsRepository, TemporalDynamicSingleWordsManager>();
			services.AddScoped<ITemporalRelationalWordsRepository, TemporalDynamicRelationalWordsManager>();
			services.AddScoped<ITemporalIrregularRepository, TemporalIrregularManager>();

			services.AddScoped<IWordsRepository, WordsManager>();

			services.AddRazorPages().AddRazorRuntimeCompilation();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseDefaultFiles(new DefaultFilesOptions { DefaultFileNames = new List<string> { "index.html" } });
			app.UseStaticFiles();

			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();




			//Add our new middleware to the pipeline
			app.UseRequestResponseLogging();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}

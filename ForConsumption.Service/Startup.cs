
using System;
using System.Text;

using ForConsumption.Common;
using ForConsumption.Common.Common;
using ForConsumption.Service.DAO;

using FreeSql;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Plugins.ToolKits;

namespace ForConsumption.Service
{
    public class Startup
    {
      

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSingleton(Program.Fsql);

            services.AddMvc().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.DateFormatHandling = JsonMapper.JsonSerializerSettings.DateFormatHandling;
                x.SerializerSettings.DefaultValueHandling = JsonMapper.JsonSerializerSettings.DefaultValueHandling;
                x.SerializerSettings.DateFormatString = JsonMapper.JsonSerializerSettings.DateFormatString;
                x.SerializerSettings.NullValueHandling = JsonMapper.JsonSerializerSettings.NullValueHandling;
                x.SerializerSettings.ReferenceLoopHandling = JsonMapper.JsonSerializerSettings.ReferenceLoopHandling;
            });

            Injecter.RegisterInstance(Program.Fsql).As<IFreeSql>();




            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MemoryBufferThreshold = int.MaxValue;
                x.ValueCountLimit = int.MaxValue;
                x.BufferBody = true;
                x.MultipartHeadersCountLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
                x.BufferBodyLengthLimit = int.MaxValue;
            });


            ServiceProvider sp = new ServiceCollection().AddOptions().Configure<TokenParameter>(Configuration.GetSection(nameof(TokenParameter))).BuildServiceProvider();
            TokenParameter tokenParameter = TokenParameter.Instance=  sp.GetService<IOptions<TokenParameter>>()?.Value;


            // ���ܽ���Token����Կ  
            services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenParameter.Secret)),
                    // �Ƿ���֤������
                    ValidateIssuer = true,
                    // ����������
                    ValidIssuer = tokenParameter.Issuer,
                    // �Ƿ���֤������
                    // ����������
                    ValidateAudience = true,
                    ValidAudience = tokenParameter.Audience,
                    // �Ƿ���֤������Ч��
                    ValidateLifetime = true,
                    // ÿ�ΰ䷢���ƣ�������Чʱ��
                    ClockSkew = TimeSpan.FromMinutes(tokenParameter.AccessTimeout)
                };
            });

            InitailizedData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }




            app.UseRouting();

            app.UseAuthentication();  // ע������
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action}/{id?}");
                endpoints.MapControllers();

            });
        }


        private void InitailizedData()
        {
            MemberDataContext dataContext = new MemberDataContext();
            if (dataContext.Members.Select.Count() == 0)
            {
                dataContext.Add(new Member() { LoginName = "xujing", Password = "admin" });
                dataContext.Add(new Member() { LoginName = "huyong", Password = "admin" });
                dataContext.SaveChanges();
            }


            PaymentDataContext paymentDataContext = new PaymentDataContext();
            if (paymentDataContext.PaymentModes.Select.Count() == 0)
            {
                paymentDataContext.Add(new PaymentMode() { Mode = "֧����" });
                paymentDataContext.Add(new PaymentMode() { Mode = "΢��" });
                paymentDataContext.Add(new PaymentMode() { Mode = "�ֽ�" });
                paymentDataContext.Add(new PaymentMode() { Mode = "QQ" });
                paymentDataContext.SaveChanges();
            }


            CategoryDataContext categoryDataContext = new CategoryDataContext();
            if (categoryDataContext.CategoryModes.Select.Count() == 0)
            {
                categoryDataContext.Add(new CategoryMode() { Mode = "����" });
                categoryDataContext.Add(new CategoryMode() { Mode = "����" });
                categoryDataContext.Add(new CategoryMode() { Mode = "����" });
                categoryDataContext.Add(new CategoryMode() { Mode = "����" });
                categoryDataContext.SaveChanges();
            }
        }
    }
}

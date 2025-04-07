using pv311_web_api.BLL.Services.Account;
using pv311_web_api.BLL.Services.Cars;
using pv311_web_api.BLL.Services.Email;
using pv311_web_api.BLL.Services.Image;
using pv311_web_api.BLL.Services.JwtService;
using pv311_web_api.BLL.Services.Manufactures;
using pv311_web_api.BLL.Services.Role;
using pv311_web_api.BLL.Services.User;
using pv311_web_api.Jobs;
using Quartz;

namespace pv311_web_api.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IManufactureService, ManufactureService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IJwtService, JwtService>();
        }

        public static void AddJobs(this IServiceCollection services, params (Type type, string cronExpression)[] jobs)
        {
            services.AddQuartz(q =>
            {
                foreach (var job in jobs)
                {
                    var jobKey = new JobKey(job.type.Name);
                    q.AddJob(job.type, jobKey);

                    q.AddTrigger(opt => opt
                    .ForJob(jobKey)
                    .WithIdentity($"{job.type.Name}-trigger")
                    .WithCronSchedule(job.cronExpression));
                }
            }); 
        }
    }
}
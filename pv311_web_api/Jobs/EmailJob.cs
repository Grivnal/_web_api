using pv311_web_api.BLL.Services.Email;
using Quartz;

namespace pv311_web_api.Jobs
{
    public class EmailJob : IJob
    {
        private readonly IEmailService _emailService;

        public EmailJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _emailService.SendMailAsync("dmytro.potapchuk22@gmail.com", "Quartz text", "Email job send mail");
        }
    }
}
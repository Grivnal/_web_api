using Quartz;

namespace pv311_web_api.Jobs
{
    public class LogCleanJob : IJob
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LogCleanJob(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var logsPath = Path.Combine(_webHostEnvironment.ContentRootPath, "logs");
            var logs = Directory.GetFiles(logsPath);

            foreach (var log in logs)
            {
                var file = new FileInfo(log);
                var days = (DateTime.Now - file.CreationTime).Days;

                if(days >= 7)
                {
                    File.Delete(log);
                }
            }

            return Task.CompletedTask;
        }
    }
}
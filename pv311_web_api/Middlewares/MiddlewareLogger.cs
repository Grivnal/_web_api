namespace pv311_web_api.Middlewares
{
    public class MiddlewareLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareLogger> _logger;

        public MiddlewareLogger(RequestDelegate next, ILogger<MiddlewareLogger> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string message = $"Request: {context.Request.Path} {context.Request.Method} - {DateTime.Now}";
            _logger.LogInformation(message);

            await _next(context);

            message = $"Response: {context.Response.StatusCode} - {DateTime.Now}";
            _logger.LogInformation(message);
        }
    }   
}
using pv311_web_api.BLL.Services;

namespace pv311_web_api.Middlewares
{
    public class MiddlewareNullExceptionHandler
    {
        private readonly RequestDelegate _next;

        public MiddlewareNullExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException)
            {
                var response = new ServiceResponse("Значення є null");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
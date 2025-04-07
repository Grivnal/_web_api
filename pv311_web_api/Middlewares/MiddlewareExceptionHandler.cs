using pv311_web_api.BLL.Services;

namespace pv311_web_api.Middlewares
{
    public class MiddlewareExceptionHandler
    {
        private readonly RequestDelegate _next;

        public MiddlewareExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = new ServiceResponse(ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
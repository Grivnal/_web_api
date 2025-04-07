namespace pv311_web_api.BLL.Services
{
    public class ServiceResponse
    {
        public ServiceResponse() { }
        public ServiceResponse(string message, bool isSuccess = false, object? payload = null) 
        {
            IsSuccess = isSuccess;
            Payload = payload;
            Message = message;
        }

        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public object? Payload { get; set; }
    }
}
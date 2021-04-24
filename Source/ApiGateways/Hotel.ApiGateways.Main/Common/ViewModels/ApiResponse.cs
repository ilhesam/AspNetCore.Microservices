namespace Hotel.ApiGateways.Main.Common.ViewModels
{
    public class ApiResponse
    {
        public ApiResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse<TData> : ApiResponse
    {
        public ApiResponse(string code, string message, TData data) : base(code, message)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}
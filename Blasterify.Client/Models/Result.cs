namespace Blasterify.Client.Models
{
    /// <summary>
    /// Success
    /// Data
    /// Message
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public Result(bool success, object data, string message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}
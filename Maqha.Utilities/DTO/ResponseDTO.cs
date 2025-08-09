namespace Maqha.Utilities.DTO
{
    public class ResponseDTO<T>where T:class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string ErrorCode { get; set; }
    }
}

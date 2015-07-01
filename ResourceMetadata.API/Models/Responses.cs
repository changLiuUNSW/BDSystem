using System.Collections.Generic;

namespace ResourceMetadata.API.Models
{
    public enum ResponseStatus
    {
        Success,
        Error
    }

    public enum ResponseErrorCode
    {
        BadParameter,
        ParameterNotAccepted,
        InternalServiceError,
    }

    /// <summary>
    /// standard api response class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public Response()
        {
            Status = ResponseStatus.Success.ToString();
        }

        public string Status { get; set; }
        public T Data { get; set; }
    }

    /// <summary>
    /// standard api error response class     
    /// </summary>
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            Status = ResponseStatus.Error.ToString();
            Errors = new Dictionary<string, string>();
        }

        public string Status { get; set; }
        public IDictionary<string, string> Errors { get; set; } 
    }
}
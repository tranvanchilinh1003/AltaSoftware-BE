using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T? Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string[]>? Errors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PageSize { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalItems { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalPages { get; set; }

        public ApiResponse(int code, string message, T? data = default, Dictionary<string, string[]>? errors = null,
                           int? page = null, int? pageSize = null, int? totalItems = null, int? totalPages = null)
        {
            Code = code;
            Message = message;
            Data = data;
            Errors = errors;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }

        public static ApiResponse<T> Success(T data = default, int? page = null, int? pageSize = null, int? totalItems = null)
        {
            int? totalPages = (totalItems.HasValue && pageSize.HasValue && pageSize > 0) ?
                              (int?)Math.Ceiling((double)totalItems.Value / pageSize.Value) : null;
            return new ApiResponse<T>(0, "Success", data, null, page, pageSize, totalItems, totalPages);
        }

        public static ApiResponse<T> Error(Dictionary<string, string[]> errors)
        {
            return new ApiResponse<T>(1, "Errors", default, errors);
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>(1, message);
        }

        public static ApiResponse<T> NotFound(string message = "Not found")
        {
            return new ApiResponse<T>(1, message);
        }

        public static ApiResponse<T> BadRequest(string message = "Bad request")
        {
            return new ApiResponse<T>(1, message);
        }

        public static ApiResponse<T> Conflict(string message = "Conflict")
        {
            return new ApiResponse<T>(1, message);
        }

        public static ApiResponse<T> Unauthorized(string message = "Unauthorized")
        {
            return new ApiResponse<T>(1, message);
        }

        internal static ApiResponse<bool> Error(string message)
        {
            throw new NotImplementedException();
        }
    }
}

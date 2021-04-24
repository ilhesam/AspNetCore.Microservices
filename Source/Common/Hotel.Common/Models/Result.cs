using System;
using Newtonsoft.Json;

namespace Hotel.Common.Models
{
    [Serializable]
    public class Result<TData> where TData : class
    {
        // For deserialize
        private Result()
        {
        }

        private Result(TData data)
        {
            Error = null;
            IsSucceeded = true;
            Data = data;
        }

        private Result(Error error)
        {
            Error = error;
            IsSucceeded = false;
            Data = default;
        }

        [JsonProperty(nameof(Error))] public Error Error { get; private set; } // private set for deserialize
        [JsonProperty(nameof(IsSucceeded))] public bool IsSucceeded { get; private set; } // private set for deserialize

        [JsonProperty(nameof(Data))] public TData Data { get; private set; } // private set for deserialize

        public static Result<TData> Success(TData data) => new(data);
        public static Result<TData> Fail(Error error) => new(error);
    }
}
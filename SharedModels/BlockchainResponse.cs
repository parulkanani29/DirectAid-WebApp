using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharedModels
{
    public class BlockchainResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }

    public class Errors
    {
        public int status { get; set; }
        public string message { get; set; }
        public string description { get; set; }

    }
    public class BlockchainError
    {
        public IList<Errors> errors { get; set; }

    }
}

﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharedModels
{
    public class ApiLocalCallResponse<T>
    {
        [JsonProperty("internalTransfers")]
        public List<object> InternalTransfers { get; set; }

        [JsonProperty("gasConsumed")]
        public GasConsumed GasConsumed { get; set; }

        [JsonProperty("revert")]
        public bool Revert { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("return")]
        public T Return { get; set; }

        [JsonProperty("longs")]
        public List<object> Logs { get; set; }
    }

    public class GasConsumed
    {
        [JsonProperty("value")]
        public int Value { get; set; }
    }
}

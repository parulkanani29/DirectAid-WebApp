using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharedModels
{
    public class ReceiptSearch
    {
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }

        [JsonProperty("postState")]
        public string PostState { get; set; }

        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("newContractAddress")]
        public object NewContractAddress { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("returnValue")]
        public string ReturnValue { get; set; }

        [JsonProperty("bloom")]
        public string Bloom { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("logs")]
        public List<Log> Logs { get; set; }
    }

    public partial class Log
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("topics")]
        public string[] Topics { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("log")]
        public LogDetail LogDetail { get; set; }
    }

    public partial class LogDetail
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }
    }

}

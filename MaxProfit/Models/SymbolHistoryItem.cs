using System;
using Newtonsoft.Json;

namespace MaxProfit.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SymbolHistoryItem
    {
        #region Public Properties

        [JsonProperty]
        public DateTime Date { get; set; }

        [JsonProperty("hv")]
        public HistoricalVolitility HistoricalVolitility { get; set; }

        [JsonProperty("iv")]
        public ImpliedVolitility ImpliedVolitility { get; set; }

        [JsonProperty]
        public Price Price { get; set; }

        [JsonProperty]
        public Volume Volume { get; set; }

        #endregion Public Properties
    }
}
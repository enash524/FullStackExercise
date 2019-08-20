using Newtonsoft.Json;

namespace MaxProfit.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class HistoricalVolitility
    {
        #region Public Properties

        [JsonProperty]
        public decimal Hv10 { get; set; }

        [JsonProperty]
        public decimal Hv120 { get; set; }

        [JsonProperty]
        public decimal Hv180 { get; set; }

        [JsonProperty]
        public decimal Hv20 { get; set; }

        [JsonProperty]
        public decimal Hv30 { get; set; }

        [JsonProperty]
        public decimal Hv360 { get; set; }

        [JsonProperty]
        public decimal Hv60 { get; set; }

        [JsonProperty]
        public decimal Hv90 { get; set; }

        #endregion Public Properties
    }
}
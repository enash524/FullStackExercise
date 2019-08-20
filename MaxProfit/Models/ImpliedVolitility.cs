using Newtonsoft.Json;

namespace MaxProfit.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ImpliedVolitility
    {
        #region Public Properties

        [JsonProperty]
        public decimal Iv120 { get; set; }

        [JsonProperty]
        public decimal Iv180 { get; set; }

        [JsonProperty]
        public decimal Iv30 { get; set; }

        [JsonProperty]
        public decimal Iv360 { get; set; }

        [JsonProperty]
        public decimal Iv60 { get; set; }

        [JsonProperty]
        public decimal Iv90 { get; set; }

        #endregion Public Properties
    }
}
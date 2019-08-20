using Newtonsoft.Json;

namespace MaxProfit.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Price
    {
        #region Public Properties

        [JsonProperty]
        public decimal Close { get; set; }

        [JsonProperty]
        public decimal High { get; set; }

        [JsonProperty]
        public decimal Low { get; set; }

        [JsonProperty]
        public decimal Open { get; set; }

        #endregion Public Properties
    }
}
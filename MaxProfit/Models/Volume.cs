using Newtonsoft.Json;

namespace MaxProfit.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Volume
    {
        #region Public Properties

        [JsonProperty("calls_volume")]
        public int CallsVolume { get; set; }

        [JsonProperty("puts_volume")]
        public int PutsVolume { get; set; }

        [JsonProperty("stock_volume")]
        public int StockVolume { get; set; }

        [JsonProperty("total_options_volume")]
        public int TotalOptionsVolume { get; set; }

        #endregion Public Properties
    }
}
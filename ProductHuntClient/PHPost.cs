using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHuntClient
{
    [JsonObject]
    public sealed class PHPost
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("thumbnail")]
        public ThumbnailObject Thumbnail { get; set; }


    }

    [JsonObject]
    public sealed class ThumbnailObject
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    
}

using Newtonsoft.Json;

namespace Annotation.Models
{
    public class BoundingBox
    {
        public int Id { get; set; }

        [JsonProperty("ClassId")]
        public int ClassId { get; set; }

        [JsonProperty("X")]
        public int X { get; set; }

        [JsonProperty("Y")]
        public int Y { get; set; }

        [JsonProperty("Width")]
        public int Width { get; set; }

        [JsonProperty("Height")]
        public int Height { get; set; }

        public Class Class { get; set; }

        [JsonProperty("ClassName")]
        public string ClassName { get; set; }
    }
}

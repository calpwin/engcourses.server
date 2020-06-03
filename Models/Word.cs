using Newtonsoft.Json;

namespace EngCourses.Models
{
    public class Word
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("eng")]
        public string Eng { get; set; }

        [JsonProperty("rus")]
        public string Rus { get; set; }

        [JsonProperty("enex")]
        public string Enex { get; set; }

        [JsonProperty("ruex")]
        public string Ruex { get; set; }

        [JsonProperty("picurl")]
        public string Picurl { get; set; }

        [JsonProperty("picau")]
        public string Picau { get; set; }

        [JsonProperty("gap")]
        public long Gap { get; set; }

        [JsonProperty("engText")]
        public string EngText { get; set; }

        [JsonProperty("engImage")]
        public string EngImage { get; set; }

        [JsonProperty("engAudio")]
        public string EngAudio { get; set; }

        [JsonProperty("engAudioExtend")]
        public string EngAudioExtend { get; set; }
        
        [JsonProperty("exception", NullValueHandling = NullValueHandling.Ignore )]
        public string Exception { get; set; }
    }
}
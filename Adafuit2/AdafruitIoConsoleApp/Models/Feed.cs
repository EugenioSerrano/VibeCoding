using System.Text.Json.Serialization;

namespace AdafruitIoConsoleApp.Models
{
    public class Feed
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("last_value")]
        public string? LastValue { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    public class FeedData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }
    }
}

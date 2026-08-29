using System.Text.Json.Serialization;

namespace FriendsForeverChangeRequest.Models
{

    public enum MemberRole
    {
        PRODUCER,
        FINANCE,
        EDITOR,
        CREATOR
    }

    public class Member
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public MemberRole Role { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}

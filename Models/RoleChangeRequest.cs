using System.Text.Json.Serialization;

namespace FriendsForeverChangeRequest.Models
{

    public class RoleChangeRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("requester_id")]
        public string RequesterId { get; set; } = string.Empty;

        [JsonPropertyName("target_id")]
        public string TargetId { get; set; } = string.Empty;

        [JsonPropertyName("new_role")]
        public MemberRole NewRole { get; set; }

        [JsonPropertyName("status")]
        public RequestStatus Status { get; set; } = RequestStatus.PENDING;

        [JsonIgnore]
        public List<Decision> Decisions { get; set; } = new();
    }
}

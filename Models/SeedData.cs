using System.Text.Json.Serialization;

namespace FriendsForeverChangeRequest.Models
{

    public class SeedData
    {
        [JsonPropertyName("members")]
        public List<Member> Members { get; set; } = new();

        [JsonPropertyName("role_change_requests")]
        public List<RoleChangeRequest> RoleChangeRequests { get; set; } = new();

        [JsonPropertyName("decisions")]
        public List<Decision> Decisions { get; set; } = new();
    }
}

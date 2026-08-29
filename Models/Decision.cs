using System.Text.Json.Serialization;

namespace FriendsForeverChangeRequest.Models
{

    public class Decision
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = string.Empty;

        [JsonPropertyName("member_id")]
        public string MemberId { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public DecisionResult Result { get; set; }
    }
}

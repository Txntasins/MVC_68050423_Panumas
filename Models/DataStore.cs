using System.Text.Json;
using System.Text.Json.Serialization;

namespace FriendsForeverChangeRequest.Models
{

    public class DataStore
    {
        public List<Member> Members { get; private set; } = new();
        public List<RoleChangeRequest> Requests { get; private set; } = new();
        public List<Decision> Decisions { get; private set; } = new();

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public void LoadFromFile(string path)
        {
            var json = File.ReadAllText(path);
            var seed = JsonSerializer.Deserialize<SeedData>(json, JsonOptions)
                       ?? throw new InvalidOperationException("Failed to read seed_data.json.");

            Members = seed.Members;
            Requests = seed.RoleChangeRequests;
            Decisions = seed.Decisions;

            foreach (var decision in Decisions)
            {
                var request = Requests.FirstOrDefault(r => r.Id == decision.RequestId);
                request?.Decisions.Add(decision);
            }
        }

        public Member? FindMember(string id) => Members.FirstOrDefault(m => m.Id == id);

        public RoleChangeRequest? FindRequest(string id) => Requests.FirstOrDefault(r => r.Id == id);

        public string NextRequestId()
        {
            var maxNum = Requests
                .Select(r => int.TryParse(r.Id.TrimStart('C'), out var n) ? n : 0)
                .DefaultIfEmpty(0)
                .Max();
            return $"C{maxNum + 1:D2}";
        }
    }
}

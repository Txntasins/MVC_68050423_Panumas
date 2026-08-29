using FriendsForeverChangeRequest.Models;

namespace FriendsForeverChangeRequest.Controllers
{

    public class SummaryController
    {
        private readonly DataStore _store;

        public SummaryController(DataStore store)
        {
            _store = store;
        }

        public Dictionary<RequestStatus, List<RoleChangeRequest>> GetRequestsByStatus()
        {
            return _store.Requests
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public (int approvals, int rejections) GetVoteCounts(string requestId)
        {
            var request = _store.FindRequest(requestId);
            if (request is null) return (0, 0);

            return (
                request.Decisions.Count(d => d.Result == DecisionResult.APPROVE),
                request.Decisions.Count(d => d.Result == DecisionResult.REJECT)
            );
        }
    }
}

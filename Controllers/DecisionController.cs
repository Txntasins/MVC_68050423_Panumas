using FriendsForeverChangeRequest.Models;

namespace FriendsForeverChangeRequest.Controllers
{
    public class DecisionController
    {
        private const int DecisionsNeededToFinalize = 2;

        private readonly DataStore _store;

        public DecisionController(DataStore store)
        {
            _store = store;
        }

        public List<Member> GetEligibleDeciders(string requestId)
        {
            var request = _store.FindRequest(requestId);
            if (request is null) return new List<Member>();

            return _store.Members
                .Where(m => m.Active && m.Id != request.RequesterId && m.Id != request.TargetId)
                .ToList();
        }

        public OperationResult Submit(string requestId, string memberId, DecisionResult result)
        {
            var request = _store.FindRequest(requestId);
            var member = _store.FindMember(memberId);

            if (request is null || member is null)
                return OperationResult.Fail("Request or member not found.");

            if (request.Status != RequestStatus.PENDING)
                return OperationResult.Fail("This request has already been finalized. No further decisions can be submitted.");

            if (!member.Active)
                return OperationResult.Fail("This member is not Active and is not eligible to submit a decision.");

            if (memberId == request.RequesterId || memberId == request.TargetId)
                return OperationResult.Fail("The requester and the target member are not eligible to decide on their own request.");

            if (request.Decisions.Any(d => d.MemberId == memberId))
                return OperationResult.Fail("This member has already submitted a decision for this request. Duplicate decisions are not allowed.");

            var decision = new Decision { RequestId = requestId, MemberId = memberId, Result = result };
            request.Decisions.Add(decision);
            _store.Decisions.Add(decision);

            FinalizeIfPossible(request);

            return OperationResult.Ok($"Decision \"{result}\" recorded for request {requestId} (current status: {request.Status}).");
        }

        private void FinalizeIfPossible(RoleChangeRequest request)
        {
            int approvals = request.Decisions.Count(d => d.Result == DecisionResult.APPROVE);
            int rejections = request.Decisions.Count(d => d.Result == DecisionResult.REJECT);

            if (approvals >= DecisionsNeededToFinalize)
            {
                request.Status = RequestStatus.APPROVED;
                var target = _store.FindMember(request.TargetId);
                if (target is not null)
                    target.Role = request.NewRole;
            }
            else if (rejections >= DecisionsNeededToFinalize)
            {
                request.Status = RequestStatus.REJECTED;
            }
        }
    }
}

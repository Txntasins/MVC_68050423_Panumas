using FriendsForeverChangeRequest.Models;

namespace FriendsForeverChangeRequest.Controllers
{
    public class RequestController
    {
        private readonly DataStore _store;

        public RequestController(DataStore store)
        {
            _store = store;
        }

        public List<RoleChangeRequest> GetAllRequests() => _store.Requests;

        public RoleChangeRequest? GetRequest(string id) => _store.FindRequest(id);

        public OperationResult CreateRequest(string requesterId, string targetId, MemberRole newRole)
        {
            var requester = _store.FindMember(requesterId);
            var target = _store.FindMember(targetId);

            if (requester is null || target is null)
                return OperationResult.Fail("Member not found.");

            if (requesterId == targetId)
                return OperationResult.Fail("The requester cannot be the target of their own request.");

            bool targetHasPendingRequest = _store.Requests.Any(r =>
                r.TargetId == targetId && r.Status == RequestStatus.PENDING);

            if (targetHasPendingRequest)
                return OperationResult.Fail("The target member already has a \"Pending\" request. Duplicate requests are not allowed.");

            var request = new RoleChangeRequest
            {
                Id = _store.NextRequestId(),
                RequesterId = requesterId,
                TargetId = targetId,
                NewRole = newRole,
                Status = RequestStatus.PENDING
            };

            _store.Requests.Add(request);
            return OperationResult.Ok($"Request {request.Id} created successfully. Status: \"Pending\".");
        }

        public OperationResult CancelRequest(string requestId, string actorId)
        {
            var request = _store.FindRequest(requestId);
            if (request is null)
                return OperationResult.Fail("Request not found.");

            if (request.RequesterId != actorId)
                return OperationResult.Fail("Only the original requester can cancel this request.");

            if (request.Status != RequestStatus.PENDING)
                return OperationResult.Fail("Cannot cancel: this request has already been finalized.");

            if (request.Decisions.Count > 0)
                return OperationResult.Fail("Cannot cancel: decisions have already been submitted for this request.");

            request.Status = RequestStatus.CANCELLED;
            return OperationResult.Ok($"Request {requestId} cancelled successfully.");
        }
    }
}

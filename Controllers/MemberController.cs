using FriendsForeverChangeRequest.Models;

namespace FriendsForeverChangeRequest.Controllers
{

    public class MemberController
    {
        private readonly DataStore _store;

        public MemberController(DataStore store)
        {
            _store = store;
        }

        public List<Member> GetAllMembers() => _store.Members;

        public Member? GetMember(string id) => _store.FindMember(id);
    }
}

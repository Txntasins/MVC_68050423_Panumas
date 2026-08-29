using Avalonia.Controls;
using Avalonia.Interactivity;
using FriendsForeverChangeRequest.Controllers;
using FriendsForeverChangeRequest.Models;

namespace FriendsForeverChangeRequest.Views
{

    public partial class MainWindow : Window
    {
        private readonly MemberController _memberController;
        private readonly RequestController _requestController;
        private readonly DecisionController _decisionController;
        private readonly SummaryController _summaryController;

        public MainWindow() : this(null!, null!, null!, null!) { }

        public MainWindow(
            MemberController memberController,
            RequestController requestController,
            DecisionController decisionController,
            SummaryController summaryController)
        {
            InitializeComponent();

            _memberController = memberController;
            _requestController = requestController;
            _decisionController = decisionController;
            _summaryController = summaryController;

            if (_memberController is not null)
            {
                RefreshMembersAndRequests();
            }
        }

        private void ShowMessage(string message) => StatusText.Text = message;

        private void OnRefreshMembersClicked(object? sender, RoutedEventArgs e)
        {
            RefreshMembersAndRequests();
            ShowMessage("Members and requests reloaded.");
        }

        private void RefreshMembersAndRequests()
        {
            var members = _memberController.GetAllMembers();
            MembersGrid.ItemsSource = members.Select(m => new MemberRow(m)).ToList();

            var requests = _requestController.GetAllRequests();
            RequestsGrid.ItemsSource = requests.Select(r => new RequestRow(r)).ToList();
        }

        private void OnCreateRequestClicked(object? sender, RoutedEventArgs e)
        {
            var requesterId = CreateRequesterIdBox.Text?.Trim() ?? string.Empty;
            var targetId = CreateTargetIdBox.Text?.Trim() ?? string.Empty;
            var roleItem = CreateNewRoleBox.SelectedItem as ComboBoxItem;
            var roleText = roleItem?.Content as string;

            if (string.IsNullOrWhiteSpace(roleText) ||
                !Enum.TryParse<MemberRole>(roleText, true, out var newRole))
            {
                ShowMessage("Please select a valid new role.");
                return;
            }

            var result = _requestController.CreateRequest(requesterId, targetId, newRole);
            ShowMessage(result.Message);

            if (result.Success)
            {
                CreateRequesterIdBox.Text = string.Empty;
                CreateTargetIdBox.Text = string.Empty;
                CreateNewRoleBox.SelectedItem = null;
                RefreshMembersAndRequests();
            }
        }

        private void OnShowEligibleClicked(object? sender, RoutedEventArgs e)
        {
            var requestId = DecisionRequestIdBox.Text?.Trim() ?? string.Empty;
            var eligible = _decisionController.GetEligibleDeciders(requestId);

            if (eligible.Count == 0)
            {
                ShowMessage($"Request {requestId} not found, or no eligible deciders.");
            }
            else
            {
                ShowMessage($"Found {eligible.Count} eligible decider(s) for request {requestId}.");
            }

            EligibleList.ItemsSource = eligible
                .Select(m => $"{m.Id}  {m.Name}  ({m.Role})")
                .ToList();
        }

        private void OnSubmitDecisionClicked(object? sender, RoutedEventArgs e)
        {
            var requestId = DecisionRequestIdBox.Text?.Trim() ?? string.Empty;
            var memberId = DecisionMemberIdBox.Text?.Trim() ?? string.Empty;
            var resultItem = DecisionResultBox.SelectedItem as ComboBoxItem;
            var resultText = resultItem?.Content as string;

            if (string.IsNullOrWhiteSpace(resultText) ||
                !Enum.TryParse<DecisionResult>(resultText, true, out var decisionResult))
            {
                ShowMessage("Please select a valid decision (APPROVE/REJECT).");
                return;
            }

            var result = _decisionController.Submit(requestId, memberId, decisionResult);
            ShowMessage(result.Message);

            if (result.Success)
            {
                DecisionMemberIdBox.Text = string.Empty;
                DecisionResultBox.SelectedItem = null;
                OnShowEligibleClicked(sender, e);
                RefreshMembersAndRequests();
            }
        }

        private void OnCancelRequestClicked(object? sender, RoutedEventArgs e)
        {
            var requestId = CancelRequestIdBox.Text?.Trim() ?? string.Empty;
            var actorId = CancelActorIdBox.Text?.Trim() ?? string.Empty;

            var result = _requestController.CancelRequest(requestId, actorId);
            ShowMessage(result.Message);

            if (result.Success)
            {
                CancelRequestIdBox.Text = string.Empty;
                CancelActorIdBox.Text = string.Empty;
                RefreshMembersAndRequests();
            }
        }

        private void OnRefreshSummaryClicked(object? sender, RoutedEventArgs e)
        {
            var byStatus = _summaryController.GetRequestsByStatus();
            var members = _memberController.GetAllMembers();

            var statusLines = Enum.GetValues<RequestStatus>().Select(status =>
            {
                var list = byStatus.TryGetValue(status, out var v) ? v : new List<RoleChangeRequest>();
                var ids = string.Join(", ", list.Select(r => r.Id));
                return $"- {status}: {list.Count} request(s) ({ids})";
            }).ToList();

            StatusSummaryList.ItemsSource = statusLines;
            MemberRolesGrid.ItemsSource = members.Select(m => new MemberRow(m)).ToList();

            ShowMessage("Latest summary displayed.");
        }

        private sealed class MemberRow
        {
            public string Id { get; }
            public string Name { get; }
            public MemberRole Role { get; }
            public string Status { get; }

            public MemberRow(Member m)
            {
                Id = m.Id;
                Name = m.Name;
                Role = m.Role;
                Status = m.Active ? "Active" : "Inactive";
            }
        }

        private sealed class RequestRow
        {
            public string Id { get; }
            public string Who { get; }
            public MemberRole NewRole { get; }
            public RequestStatus Status { get; }

            public RequestRow(RoleChangeRequest r)
            {
                Id = r.Id;
                Who = $"{r.RequesterId} -> {r.TargetId}";
                NewRole = r.NewRole;
                Status = r.Status;
            }
        }
    }
}

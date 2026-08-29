using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FriendsForeverChangeRequest.Controllers;
using FriendsForeverChangeRequest.Models;
using FriendsForeverChangeRequest.Views;

namespace FriendsForeverChangeRequest
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                var store = new DataStore();
                store.LoadFromFile(Path.Combine(AppContext.BaseDirectory, "seed_data.json"));

                var memberController = new MemberController(store);
                var requestController = new RequestController(store);
                var decisionController = new DecisionController(store);
                var summaryController = new SummaryController(store);

                desktop.MainWindow = new MainWindow(
                    memberController,
                    requestController,
                    decisionController,
                    summaryController);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}

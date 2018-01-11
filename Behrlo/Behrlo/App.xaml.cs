using System.Threading.Tasks;
using Behrlo.Services;
using Windows.ApplicationModel.Activation;
using Template10.Common;
using Windows.UI.Xaml.Data;
using Behrlo.Views;
using Behrlo.Models;
using Windows.UI.Xaml;
using Template10.Controls;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;

namespace Behrlo
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Splash(e);

            #region App settings

            var settings = SettingsService.Instance;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;

            #endregion App settings
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e) =>
            new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Splash(e.SplashScreen)
            };

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            using (var db = new ApplicationDbContext())
            {
                await db.InitializeDatabaseAsync();
            }

            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude, new Frame());
            ModalDialog.Content = new Shell(service);
            await NavigationService.NavigateAsync(typeof(NotebooksPage));
        }
    }
}

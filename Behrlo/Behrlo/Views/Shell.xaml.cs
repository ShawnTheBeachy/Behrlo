using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Behrlo.Views
{
    public sealed partial class Shell : UserControl
    {
        private INavigationService _navigationService;
        public static Shell Instance { get; set; }

        #region ApplicationTitle

        public string ApplicationTitle
        {
            get => (string)GetValue(ApplicationTitleProperty);
            set => SetValue(ApplicationTitleProperty, value);
        }
        
        public static readonly DependencyProperty ApplicationTitleProperty =
            DependencyProperty.Register("ApplicationTitle", typeof(string), typeof(Shell), new PropertyMetadata("Behrlo"));

        #endregion ApplicationTitle

        public Shell()
        {
            Instance = this;
            InitializeComponent();
        }

        public Shell(INavigationService navigationService) : this() =>
            SetNavigationService(navigationService);

        public void SetNavigationService(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ContentGrid.Children.Add(_navigationService.Frame);
            _navigationService.FrameFacade.Navigated += FrameFacade_Navigated;
        }

        private void FrameFacade_Navigated(object sender, NavigatedEventArgs e)
        {
            if (SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Collapsed)
                AppTitle.Margin = new Thickness(12, 8, 0, 8);
            else
                AppTitle.Margin = new Thickness(60, 8, 0, 8);
        }

        public void SetApplicationTitle(string title)
        {
            ApplicationTitle = string.IsNullOrEmpty(title) ? "Behrlo" : $"{title} - Behrlo";
        }
    }
}

using System;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Template10.Utils;
using Windows.UI.Xaml;

namespace Behrlo.Services
{
    public class SettingsService : BindableBase
    {
        public static SettingsService Instance { get; } = new SettingsService();
        private ISettingsHelper _helper;

        private SettingsService()
        {
            _helper = new SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get => _helper.Read(nameof(UseShellBackButton), true);
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                BootStrapper.Current.NavigationService.GetDispatcherWrapper().Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                });
            }
        }
        
        public TimeSpan CacheMaxDuration
        {
            get => _helper.Read(nameof(CacheMaxDuration), TimeSpan.FromDays(2));
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                BootStrapper.Current.CacheMaxDuration = value;
            }
        }
    }
}

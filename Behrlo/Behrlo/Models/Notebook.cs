using System;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using Windows.UI.Xaml.Media.Imaging;

namespace Behrlo.Models
{
    public class Notebook : BindableBase
    {
        #region Id

        private Guid _id = Guid.NewGuid();
        /// <summary>
        /// The unique identifier for this notebook.
        /// </summary>
        public Guid Id { get => _id; set => Set(ref _id, value); }

        #endregion Id

        #region CoverImage

        private BitmapImage _coverImage;
        public BitmapImage CoverImage { get => _coverImage; set => Set(ref _coverImage, value); }

        #endregion CoverImage

        #region Name

        private string _name = default(string);
        /// <summary>
        /// The name of the notebook.
        /// </summary>
        public string Name { get => _name; set => Set(ref _name, value); }

        #endregion Name

        #region Sections

        private ObservableCollection<Section> _sections = new ObservableCollection<Section>();
        /// <summary>
        /// The sections in the notebook.
        /// </summary>
        public virtual ObservableCollection<Section> Sections { get => _sections; set => Set(ref _sections, value); }

        #endregion Sections
    }
}

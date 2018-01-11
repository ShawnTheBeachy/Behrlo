using Behrlo.Models;
using Behrlo.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Behrlo.Views
{
    public sealed partial class NotebooksPage : Page
    {
        public NotebooksPage()
        {
            InitializeComponent();
        }

        private async void ChangeNotebookCoverMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var notebook = (sender as FrameworkElement).DataContext as Notebook;

            var coverPicker = new FileOpenPicker
            {
                SettingsIdentifier = $"com.tastesliketurkey.behrlo.notebookcoverpicker.{notebook.Id.ToString()}",
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };
            coverPicker.FileTypeFilter.Add(".jpg");
            coverPicker.FileTypeFilter.Add(".jpeg");
            coverPicker.FileTypeFilter.Add(".png");
            coverPicker.FileTypeFilter.Add(".bmp");
            coverPicker.FileTypeFilter.Add(".gif");

            var coverFile = await coverPicker.PickSingleFileAsync();

            if (coverFile == null)
                return;

            await coverFile.CopyAsync(ApplicationData.Current.LocalFolder, notebook.Id.ToString(), NameCollisionOption.ReplaceExisting);
            var bitmap = new BitmapImage();

            using (var stream = await coverFile.OpenReadAsync())
            {
                await bitmap.SetSourceAsync(stream);
                notebook.CoverImage = bitmap;
            }
        }

        private void DeleteNotebookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var notebook = (sender as FrameworkElement).DataContext as Notebook;

            if (ViewModel.DeleteNotebookCommand.CanExecute(notebook))
                ViewModel.DeleteNotebookCommand.Execute(notebook);
        }

        private async void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var notebook = textBox.DataContext as Notebook;

            if (!ViewModel.Notebooks.Contains(notebook))        // This fires when you delete a notebook, for some reason.
                return;

            notebook.Name = textBox.Text;
            await NotebooksService.ChangeNotebookAsync(notebook);
        }

        private void NotebookPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void NotebooksGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var notebook = e.ClickedItem as Notebook;

            if (ViewModel.SelectNotebookCommand.CanExecute(notebook))
                ViewModel.SelectNotebookCommand.Execute(notebook);
        }

        private async void CoverImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            await (sender as FrameworkElement).Saturation(0, 3000, 50).StartAsync();
        }

        private async void CoverImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            await (sender as FrameworkElement).Saturation(1, 1000).StartAsync();
        }
    }
}

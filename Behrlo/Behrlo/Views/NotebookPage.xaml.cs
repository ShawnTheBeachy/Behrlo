using Behrlo.Models;
using System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Behrlo.Views
{
    public sealed partial class NotebookPage : Page
    {
        private DispatcherTimer _saveTimer;

        public NotebookPage()
        {
            InitializeComponent();
            RecreateSaveTimer();
        }

        private void AddNewSectionButton_Click(object sender, RoutedEventArgs e)
        {
            NewSectionFlyout.Hide();
        }

        private void AddNewWordButton_Click(object sender, RoutedEventArgs e)
        {
            NewWordFlyout.Hide();
        }

        private void AddSectionButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewSection = new Section()
                .NotebookId(ViewModel.Notebook.Id);
            NewSectionFlyout.ShowAt(sender as FrameworkElement);
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewWord = new Word()
                .SectionId(ViewModel.SelectedSection.Id);
            NewWordFlyout.ShowAt(sender as FrameworkElement);
        }

        private void CancelNewSectionButton_Click(object sender, RoutedEventArgs e)
        {
            NewSectionFlyout.Hide();
            NewSectionTextBox.Text = string.Empty;
        }

        private void CancelNewWordButton_Click(object sender, RoutedEventArgs e)
        {
            NewWordFlyout.Hide();
            NewWordTextBox.Text = string.Empty;
            NewWordTranslationTextBox.Text = string.Empty;
        }
        
        private void DeleteSectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var section = (sender as FrameworkElement).DataContext as Section;

            if (ViewModel.DeleteSectionCommand.CanExecute(section))
                ViewModel.DeleteSectionCommand.Execute(section);
        }

        private void DeleteWordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var word = (sender as FrameworkElement).DataContext as Word;

            if (ViewModel.DeleteWordCommand.CanExecute(word))
                ViewModel.DeleteWordCommand.Execute(word);
        }

        private void DrawingInkCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            var inkCanvas = sender as InkCanvas;
            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            var drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Color.FromArgb(255, 88, 89, 91);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);

            inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        private void DrawingInkCanvas_Unloaded(object sender, RoutedEventArgs e)
        {
            var inkCanvas = sender as InkCanvas;
            inkCanvas.InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
            inkCanvas.InkPresenter.StrokesErased -= InkPresenter_StrokesErased;
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            DrawingInkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
        }

        private void EraserButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DrawingInkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            _saveTimer.Stop();
            RecreateSaveTimer();
            _saveTimer.Start();
        }

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            _saveTimer.Stop();
            RecreateSaveTimer();
            _saveTimer.Start();
        }

        private void RecreateSaveTimer()
        {
            _saveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(400)
            };
            _saveTimer.Tick += (s, e) => StrokesModified();
        }

        private void RenameSectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var section = (sender as FrameworkElement).DataContext as Section;
            ViewModel.RenamingSection = section;
            ViewModel.RenamingSectionName = section.Name;
            var sectionContainer = SectionsListView.ContainerFromItem(section);
            RenameSectionFlyout.ShowAt(sectionContainer as FrameworkElement);
            RenameSectionTextBox.SelectAll();
        }

        private void RenameSectionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel.RenameSectionCommand.CanExecute(ViewModel.RenamingSection))
                ViewModel.RenameSectionCommand.Execute(ViewModel.RenamingSection);
        }

        private void StrokesModified()
        {
            _saveTimer.Stop();

            if (ViewModel.SaveDrawingCommand.CanExecute(ViewModel.WordStrokeContainer))
                ViewModel.SaveDrawingCommand.Execute(ViewModel.WordStrokeContainer);
            else
            {
                RecreateSaveTimer();
                _saveTimer.Start();
            }
        }
    }
}

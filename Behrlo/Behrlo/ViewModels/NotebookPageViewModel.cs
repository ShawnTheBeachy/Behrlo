using System.Collections.Generic;
using System.Threading.Tasks;
using Behrlo.Models;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using System;
using Behrlo.Services;
using System.Linq;
using Template10.Utils;
using Windows.UI.Input.Inking;
using Windows.Storage;
using Behrlo.Views;

namespace Behrlo.ViewModels
{
    public class NotebookPageViewModel : ViewModelBase
    {
        public const string NOTEBOOK_ID = nameof(NOTEBOOK_ID);

        #region AddSectionCommand

        private bool _canAddSection = true;
        private DelegateCommand<Section> _addSectionCommand;
        public DelegateCommand<Section> AddSectionCommand =>
            _addSectionCommand ?? (_addSectionCommand = new DelegateCommand<Section>(async (section) =>
            {
                _canAddSection = false;
                AddSectionCommand.RaiseCanExecuteChanged();

                Notebook.Sections.Add(section);
                await SectionsService.CreateSectionAsync(section);

                _canAddSection = true;
                AddSectionCommand.RaiseCanExecuteChanged();
            }, (section) => _canAddSection && section != null));

        #endregion AddSectionCommand

        #region AddWordCommand

        private bool _canAddWord = true;
        private DelegateCommand<Word> _addWordCommand;
        public DelegateCommand<Word> AddWordCommand =>
            _addWordCommand ?? (_addWordCommand = new DelegateCommand<Word>(async (word) =>
            {
                _canAddWord = false;
                AddWordCommand.RaiseCanExecuteChanged();
                
                await WordsService.CreateWordAsync(word);
                SelectedSection.Words.Add(word);

                _canAddWord = true;
                AddWordCommand.RaiseCanExecuteChanged();
            }, (word) => _canAddWord && SelectedSection != null && word != null));

        #endregion AddWordCommand

        #region DeleteSectionCommand

        private bool _canDeleteSection = true;
        private DelegateCommand<Section> _deleteSectionCommand;
        public DelegateCommand<Section> DeleteSectionCommand =>
            _deleteSectionCommand ?? (_deleteSectionCommand = new DelegateCommand<Section>(async (section) =>
            {
                _canDeleteSection = false;
                DeleteSectionCommand.RaiseCanExecuteChanged();

                Notebook.Sections.Remove(section);
                await SectionsService.DeleteSectionAsync(section);

                _canDeleteSection = true;
                DeleteSectionCommand.RaiseCanExecuteChanged();
            }, (section) => _canDeleteSection && section != null && Notebook.Sections.Contains(section)));

        #endregion DeleteSectionCommand

        #region DeleteWordCommand

        private bool _canDeleteWord = true;
        private DelegateCommand<Word> _deleteWordCommand;
        public DelegateCommand<Word> DeleteWordCommand =>
            _deleteWordCommand ?? (_deleteWordCommand = new DelegateCommand<Word>(async (word) =>
            {
                _canDeleteWord = false;
                DeleteWordCommand.RaiseCanExecuteChanged();

                SelectedSection.Words.Remove(word);
                await WordsService.DeleteWordAsync(word);

                _canDeleteWord = true;
                DeleteWordCommand.RaiseCanExecuteChanged();
            }, (word) => _canDeleteWord && word != null && SelectedSection.Words.Contains(word)));

        #endregion DeleteWordCommand

        #region LoadWordCommand

        private DelegateCommand<InkStrokeContainer> _loadWordCommand;
        public DelegateCommand<InkStrokeContainer> LoadWordCommand =>
            _loadWordCommand ?? (_loadWordCommand = new DelegateCommand<InkStrokeContainer>(async (container) =>
            {
                container.Clear();

                if (SelectedWord == null)
                    return;

                var wordItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync($"{SelectedWord.Id.ToString()}.gif");

                if (wordItem is StorageFile wordFile)
                {
                    using (var sequentialRead = await wordFile.OpenSequentialReadAsync())
                    {
                        await container.LoadAsync(sequentialRead);
                    }
                }
            }));

        #endregion LoadWordCommand

        #region NewSection

        private Section _newSection = default(Section);
        public Section NewSection { get => _newSection; set => Set(ref _newSection, value); }

        #endregion NewSection

        #region NewWord

        private Word _newWord = default(Word);
        public Word NewWord { get => _newWord; set => Set(ref _newWord, value); }

        #endregion NewWord

        #region Notebook

        private Notebook _notebook = default(Notebook);
        public Notebook Notebook { get => _notebook; set => Set(ref _notebook, value); }

        #endregion Notebook

        #region SaveDrawingCommand

        private bool _canSaveDrawing = true;
        private DelegateCommand<InkStrokeContainer> _saveDrawingCommand;
        public DelegateCommand<InkStrokeContainer> SaveDrawingCommand =>
            _saveDrawingCommand ?? (_saveDrawingCommand = new DelegateCommand<InkStrokeContainer>(async (container) =>
            {
                _canSaveDrawing = false;

                var wordFile = await ApplicationData.Current.LocalFolder.CreateFileAsync($"{SelectedWord.Id.ToString()}.gif", CreationCollisionOption.ReplaceExisting);

                using (var stream = await wordFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await container.SaveAsync(stream);
                }

                _canSaveDrawing = true;
            }, (container) => _canSaveDrawing && container != null));

        #endregion SaveDrawingCommand

        #region SelectedSection

        private Section _selectedSection = default(Section);
        public Section SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection?.Words.Clear();

                if (Set(ref _selectedSection, value))
                {
                    SelectedWord = null;
                    var words = WordsService.GetWords(SelectedSection.Id);
                    SelectedSection.Words.AddRange(words);
                    AddWordCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion SelectedSection

        #region SelectedWord

        private Word _selectedWord = default(Word);
        public Word SelectedWord
        {
            get => _selectedWord;
            set
            {
                if (Set(ref _selectedWord, value))
                    LoadWordCommand.Execute(WordStrokeContainer);
            }
        }

        #endregion SelectedWord

        #region WordStrokeContainer

        private InkStrokeContainer _wordStrokeContainer = new InkStrokeContainer();
        public InkStrokeContainer WordStrokeContainer { get => _wordStrokeContainer; set => Set(ref _wordStrokeContainer, value); }

        #endregion WordStrokeContainer

        public async override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            Shell.Instance.SetApplicationTitle(null);
            await Task.CompletedTask;
        }

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (!SessionState.ContainsKey(NOTEBOOK_ID))
                throw new Exception($"Failed to find key {nameof(NOTEBOOK_ID)} in session state.");

            var notebookId = (Guid)SessionState[NOTEBOOK_ID];
            var notebooks = await NotebooksService.GetNotebooksAsync(id: notebookId, withSections: true);
            Notebook = notebooks.FirstOrDefault();
            var sections = SectionsService.GetSections(notebookId: notebookId);
            Notebook.Sections.AddRange(sections);

            await Task.CompletedTask;
        }
    }
}

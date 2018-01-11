using Behrlo.Models;
using System.Collections.ObjectModel;
using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Behrlo.Services;
using Template10.Utils;
using Behrlo.Views;
using Behrlo.Helpers.Extensions;

namespace Behrlo.ViewModels
{
    public class NotebooksPageViewModel : ViewModelBase
    {
        #region AddNotebookCommand

        private bool _canAddNotebook = true;
        private DelegateCommand _addNotebookCommand;
        public DelegateCommand AddNotebookCommand =>
            _addNotebookCommand ?? (_addNotebookCommand = new DelegateCommand(async () =>
            {
                _canAddNotebook = false;
                AddNotebookCommand.RaiseCanExecuteChanged();

                var notebook = new Notebook()
                    .Name("New notebook");
                await NotebooksService.CreateNotebookAsync(notebook);
                Notebooks.Insert(0, notebook);

                _canAddNotebook = true;
                AddNotebookCommand.RaiseCanExecuteChanged();
            }, () => _canAddNotebook));

        #endregion AddNotebookCommand

        #region DeleteNotebookCommand

        private bool _canDeleteNotebook = true;
        private DelegateCommand<Notebook> _deleteNotebookCommand;
        public DelegateCommand<Notebook> DeleteNotebookCommand =>
            _deleteNotebookCommand ?? (_deleteNotebookCommand = new DelegateCommand<Notebook>(async (notebook) =>
            {
                _canDeleteNotebook = false;
                DeleteNotebookCommand.RaiseCanExecuteChanged();

                Notebooks.Remove(notebook);
                await NotebooksService.DeleteNotebookAsync(notebook);

                _canDeleteNotebook = true;
                DeleteNotebookCommand.RaiseCanExecuteChanged();
            }, (notebook) => _canDeleteNotebook && notebook != null && Notebooks.Contains(notebook)));

        #endregion DeleteNotebookCommand

        #region Notebooks

        private ObservableCollection<Notebook> _notebooks { get; } = new ObservableCollection<Notebook>();
        public ObservableCollection<Notebook> Notebooks => _notebooks;

        #endregion Notebooks

        #region SelectNotebookCommand

        private bool _canSelectNotebook = true;
        private DelegateCommand<Notebook> _selectNotebookCommand;
        public DelegateCommand<Notebook> SelectNotebookCommand =>
            _selectNotebookCommand ?? (_selectNotebookCommand = new DelegateCommand<Notebook>(async (notebook) =>
            {
                _canSelectNotebook = false;
                SelectNotebookCommand.RaiseCanExecuteChanged();

                SessionState.SafeAdd(NotebookPageViewModel.NOTEBOOK_ID, notebook.Id);
                await NavigationService.NavigateAsync(typeof(NotebookPage));
                Shell.Instance.SetApplicationTitle(notebook.Name);

                _canSelectNotebook = true;
                SelectNotebookCommand.RaiseCanExecuteChanged();
            }, (notebook) => _canSelectNotebook && notebook != null));

        #endregion SelectNotebookCommand

        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (Notebooks.Count == 0)
            {
                var notebooks = await NotebooksService.GetNotebooksAsync();
                Notebooks.AddRange(notebooks);
            }

            await Task.CompletedTask;
        }
    }
}

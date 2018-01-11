using System;
using System.Collections.ObjectModel;
using Template10.Mvvm;

namespace Behrlo.Models
{
    public class Section : BindableBase
    {
        #region Id

        private Guid _id = Guid.NewGuid();
        public Guid Id { get => _id; set => Set(ref _id, value); }

        #endregion Id

        #region Name

        private string _name = default(string);
        /// <summary>
        /// The name of the section.
        /// </summary>
        public string Name { get => _name; set => Set(ref _name, value); }

        #endregion Name

        #region Notebook

        private Notebook _notebook = default(Notebook);
        /// <summary>
        /// The notebook to which the section belongs.
        /// </summary>
        public Notebook Notebook { get => _notebook; set => Set(ref _notebook, value); }

        #endregion Notebook

        #region NotebookId

        private Guid _notebookId = default(Guid);
        public Guid NotebookId { get => _notebookId; set => Set(ref _notebookId, value); }

        #endregion NotebookId

        #region Words

        private ObservableCollection<Word> _words = new ObservableCollection<Word>();
        /// <summary>
        /// The words in the section.
        /// </summary>
        public ObservableCollection<Word> Words { get => _words; set => Set(ref _words, value); }

        #endregion Words
    }
}

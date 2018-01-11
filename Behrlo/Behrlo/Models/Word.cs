using System;
using Template10.Mvvm;

namespace Behrlo.Models
{
    public class Word : BindableBase
    {
        #region Id

        private Guid _id = Guid.NewGuid();
        /// <summary>
        /// The unique identifier of the word or phrase.
        /// </summary>
        public Guid Id { get => _id; set => Set(ref _id, value); }

        #endregion Id
        
        #region Text

        private string _text = default(string);
        /// <summary>
        /// The actual text content of the word of phrase.
        /// </summary>
        public string Text { get => _text; set => Set(ref _text, value); }

        #endregion Text

        #region Section

        private Section _section = default(Section);
        /// <summary>
        /// The section to which the word belongs.
        /// </summary>
        public Section Section { get => _section; set => Set(ref _section, value); }

        #endregion Section

        #region SectionId

        private Guid _sectionId = default(Guid);
        public Guid SectionId { get => _sectionId; set => Set(ref _sectionId, value); }

        #endregion SectionId

        #region Translation

        private string _translation = default(string);
        public string Translation { get => _translation; set => Set(ref _translation, value); }

        #endregion Translation
    }
}

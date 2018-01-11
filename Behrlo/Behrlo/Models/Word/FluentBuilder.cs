using System;

namespace Behrlo.Models
{
    public static class WordFluentBuilder
    {
        /// <summary>
        /// Sets the Section property.
        /// </summary>
        /// <param name="word">The word on which to set the Section property.</param>
        /// <param name="section">The value to be assigned to the Section property.</param>
        /// <returns>The word on which the property was set.</returns>
        public static Word Section(this Word word, Section section)
        {
            word.Section = section;
            return word;
        }

        /// <summary>
        /// Sets the SectionId property.
        /// </summary>
        /// <param name="word">The word on which to set the SectionId property.</param>
        /// <param name="sectionId">The value to be assigned to the SectionId property.</param>
        /// <returns>The word on which the property was set.</returns>
        public static Word SectionId(this Word word, Guid sectionId)
        {
            word.SectionId = sectionId;
            return word;
        }

        /// <summary>
        /// Sets the Text property.
        /// </summary>
        /// <param name="word">The word on which to set the Text property.</param>
        /// <param name="text">The value to be assigned to the Text property.</param>
        /// <returns>The word on which the property was set.</returns>
        public static Word Text(this Word word, string text)
        {
            word.Text = text;
            return word;
        }

        /// <summary>
        /// Sets the Translation property.
        /// </summary>
        /// <param name="word">The word on which to set the Translation property.</param>
        /// <param name="translation">The value to be assigned to the Translation property.</param>
        /// <returns>The word on which the property was set.</returns>
        public static Word Translation(this Word word, string translation)
        {
            word.Translation = translation;
            return word;
        }
    }
}

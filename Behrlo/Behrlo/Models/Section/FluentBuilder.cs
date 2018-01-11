using System;

namespace Behrlo.Models
{
    public static class SectionFluentBuilder
    {
        /// <summary>
        /// Sets the Name property.
        /// </summary>
        /// <param name="section">The section on which to set the Name property.</param>
        /// <param name="name">The value to assign to the Name property.</param>
        /// <returns>The section on which the property was set.</returns>
        public static Section Name(this Section section, string name)
        {
            section.Name = name;
            return section;
        }

        /// <summary>
        /// Sets the NotebookId property.
        /// </summary>
        /// <param name="section">The section on which to set the NotebookId property.</param>
        /// <param name="notebookId">The value to assign to the NotebookId property.</param>
        /// <returns>The section on which the property was set.</returns>
        public static Section NotebookId(this Section section, Guid notebookId)
        {
            section.NotebookId = notebookId;
            return section;
        }
    }
}

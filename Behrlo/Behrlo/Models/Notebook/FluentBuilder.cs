using System;

namespace Behrlo.Models
{
    public static class NotebookFluentBuilder
    {
        /// <summary>
        /// Fluently sets the Id property.
        /// </summary>
        /// <param name="notebook">The notebook on which to set the property.</param>
        /// <param name="id">The value to be assigned to the Id property.</param>
        /// <returns>The notebook on which the property was set.</returns>
        public static Notebook Id(this Notebook notebook, Guid id)
        {
            notebook.Id = id;
            return notebook;
        }

        /// <summary>
        /// Fluently sets the Name property.
        /// </summary>
        /// <param name="notebook">The notebook on which to set the property.</param>
        /// <param name="name">The value to be assigned to the Name property.</param>
        /// <returns>The notebook on which the property was set.</returns>
        public static Notebook Name(this Notebook notebook, string name)
        {
            notebook.Name = name;
            return notebook;
        }
    }
}

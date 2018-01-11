using Behrlo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Behrlo.Services
{
    public static class SectionsService
    {
        /// <summary>
        /// Adds a new section to the database.
        /// </summary>
        /// <param name="section">The section to add to the database.</param>
        /// <returns>A task.</returns>
        public static async Task CreateSectionAsync(Section section)
        {
            using (var db = new ApplicationDbContext())
            {
                await db.Sections.AddAsync(section);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a section from the database.
        /// </summary>
        /// <param name="section">The notebook to delete from the section.</param>
        /// <returns>A task.</returns>
        public static async Task DeleteSectionAsync(Section section)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Sections.Remove(section);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets a list of sections from the database.
        /// </summary>
        /// <returns>A list of sections.</returns>
        public static IList<Section> GetSections(Guid? id = null, Guid? notebookId = null)
        {
            var idIsNull = !id.HasValue;
            var notebookIdIsNull = !notebookId.HasValue;

            using (var db = new ApplicationDbContext())
            {
                var sections = from section in db.Sections
                               where (
                                   (idIsNull ? true : section.Id == id) &&
                                   (notebookIdIsNull ? true : section.NotebookId == notebookId)
                               )
                               select section;

                return sections.ToList();
            }
        }
    }
}

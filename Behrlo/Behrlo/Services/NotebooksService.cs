using Behrlo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Behrlo.Services
{
    public static class NotebooksService
    {
        /// <summary>
        /// Modifies an existing notebook in the database.
        /// </summary>
        /// <param name="notebook">The modified notebook.</param>
        /// <returns>A Task representing the operation.</returns>
        public static async Task ChangeNotebookAsync(Notebook notebook)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Entry(notebook).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Adds a new notebook to the database.
        /// </summary>
        /// <param name="notebook">The notebook to add to the database.</param>
        /// <returns>A task.</returns>
        public static async Task CreateNotebookAsync(Notebook notebook)
        {
            using (var db = new ApplicationDbContext())
            {
                await db.Notebooks.AddAsync(notebook);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a notebook from the database.
        /// </summary>
        /// <param name="notebook">The notebook to delete from the database.</param>
        /// <returns>A task.</returns>
        public static async Task DeleteNotebookAsync(Notebook notebook)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Notebooks.Remove(notebook);
                await db.SaveChangesAsync();

                IStorageItem coverItem;
                coverItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(notebook.Id.ToString());

                if (coverItem is StorageFile coverFile)
                    await coverFile.DeleteAsync();
            }
        }

        /// <summary>
        /// Gets a list of notebooks from the database.
        /// </summary>
        /// <returns>A list of notebooks.</returns>
        public static async Task<IList<Notebook>> GetNotebooksAsync(Guid? id = null, bool withSections = false)
        {
            var idIsNull = !id.HasValue;

            using (var db = new ApplicationDbContext())
            {
                var notebooks = from notebook in db.Notebooks
                                where idIsNull ? true : notebook.Id == id
                                select notebook;

                if (withSections)
                    notebooks.Include(notebook => notebook.Sections);

                var list = notebooks.ToList();
                IStorageItem coverItem;

                foreach (var notebook in list)
                {
                    coverItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync(notebook.Id.ToString());

                    if (coverItem is StorageFile coverFile)
                    {
                        var bitmap = new BitmapImage();

                        using (var stream = await coverFile.OpenReadAsync())
                        {
                            await bitmap.SetSourceAsync(stream);
                            notebook.CoverImage = bitmap;
                        }
                    }
                }

                return list;
            }
        }
    }
}

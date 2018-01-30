using Behrlo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Behrlo.Services
{
    public static class WordsService
    {
        /// <summary>
        /// Adds a new word to the database.
        /// </summary>
        /// <param name="word">The word to add to the database.</param>
        /// <returns>A task.</returns>
        public static async Task CreateWordAsync(Word word)
        {
            using (var db = new ApplicationDbContext())
            {
                await db.Words.AddAsync(word);
                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Deletes a word from the database.
        /// </summary>
        /// <param name="word">The word to delete from the section.</param>
        /// <returns>A task.</returns>
        public static async Task DeleteWordAsync(Word word)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Words.Remove(word);
                await db.SaveChangesAsync();
            }

            var wordItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync($"{word.Id.ToString()}.gif");

            if (wordItem is StorageFile wordFile)
                await wordItem.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        /// <summary>
        /// Gets a list of words from the database.
        /// </summary>
        /// <returns>A list of words.</returns>
        public static IList<Word> GetWords(Guid? sectionId = null)
        {
            var sectionIdIsNull = !sectionId.HasValue;

            using (var db = new ApplicationDbContext())
            {
                var words = from word in db.Words
                            where (
                                (sectionIdIsNull ? true : word.SectionId == sectionId)
                            )
                            select word;

                return words.ToList();
            }
        }
    }
}

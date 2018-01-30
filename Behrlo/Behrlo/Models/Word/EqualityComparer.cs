using System.Collections.Generic;

namespace Behrlo.Models
{
    public class WordEqualityComparer : IEqualityComparer<Word>
    {
        public bool Equals(Word x, Word y)
        {
            return x.Id == y.Id &&
                x.SectionId == y.SectionId &&
                x.Text == y.Text &&
                x.Translation == y.Translation;
        }

        public int GetHashCode(Word obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

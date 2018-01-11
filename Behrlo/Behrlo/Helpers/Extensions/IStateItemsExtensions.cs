using Template10.Common;

namespace Behrlo.Helpers.Extensions
{
    public static class IStateItemsExtensions
    {
        public static void SafeAdd(this IStateItems state, string key, object value)
        {
            if (state.ContainsKey(key))
                state.Remove(key);

            state.Add(key, value);
        }
    }
}

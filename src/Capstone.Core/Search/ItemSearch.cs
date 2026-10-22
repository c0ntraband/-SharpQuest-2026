using SharpQuest.Contracts;

namespace Capstone.Core.Search
{
    public sealed class ItemSearch
    {
        public IReadOnlyList<IItem> Search(IItemStore store, string query)
        {
            ArgumentNullException.ThrowIfNull(store);
            ArgumentNullException.ThrowIfNull(query);

            var matcher = SearchMatcher.Create();
            return store.All().Where(item => matcher.Matches(item.DisplayName, query)).ToList();
        }
    }
}

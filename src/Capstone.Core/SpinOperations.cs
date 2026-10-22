using Capstone.Core.Models;
using SharpQuest.Contracts;

namespace Capstone.Core
{
    public sealed class SpinOperations
    {
        public string Describe(IItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (item is not Spin spin)
                throw new ArgumentException("Only spins can describe themselves here.", nameof(item));

            return spin.Describe();
        }

        public IReadOnlyList<string> DescribeAll(IItemStore store)
        {
            ArgumentNullException.ThrowIfNull(store);

            var lines = new List<string>();
            foreach (Spin spin in store.All())
                lines.Add(spin.Describe());

            return lines;
        }

        public string Classify(IItem item) => item switch
        {
            null => throw new ArgumentNullException(nameof(item)),
            PaidSpin => "paid",
            FreeSpin => "free",
            _ => "other",
        };

        public bool TryFindByName(IItemStore store, string? name, out IItem? found)
        {
            ArgumentNullException.ThrowIfNull(store);

            found = string.IsNullOrWhiteSpace(name)
                ? null
                : store.All().FirstOrDefault(item => item.DisplayName == name);
            return found is not null;
        }

        public int AddAll(IItemStore store, params IItem[] items)
        {
            ArgumentNullException.ThrowIfNull(store);
            ArgumentNullException.ThrowIfNull(items);

            var added = 0;
            foreach (var item in items)
            {
                store.Add(item);
                added++;
            }

            return added;
        }
    }
}

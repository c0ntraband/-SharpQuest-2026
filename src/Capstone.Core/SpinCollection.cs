using SharpQuest.Contracts;

namespace Capstone.Core
{
    public class SpinCollection : IItemStore
    {
        private readonly List<IItem> _spins = new();

        public int Count => _spins.Count;

        public void Add(IItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (FindById(item.Id) is not null)
                throw new ArgumentException($"A spin with Id '{item.Id}' is already stored.", nameof(item));

            _spins.Add(item);
        }
        public IReadOnlyList<IItem> All() => _spins.ToList();
        public IItem? FindById(string id) => _spins.FirstOrDefault(item => item.Id == id);

        public bool Remove(string id)
        {
            var item = FindById(id);
            return item is not null && _spins.Remove(item);
        }
    }
}

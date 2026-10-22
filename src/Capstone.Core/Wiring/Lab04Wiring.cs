using Capstone.Core.Models;
using SharpQuest.Contracts;

namespace Capstone.Core.Wiring
{
    public sealed class Lab04Wiring : ILab04Wiring
    {
        private readonly SpinOperations _operations = new();

        public IItemStore CreateStore() => new SpinCollection();

        public Type BaseType => typeof(Spin);

        public IReadOnlyList<Type> KindTypes => [typeof(PaidSpin), typeof(FreeSpin)];

        public IItem CreateOfKind(int kindIndex, int seed)
        {
            var reels = new SlotMachine(seed).NextReels();
            var time = new DateTime(2026, 10, 22, 8, 30, 0).AddSeconds(seed);
            var paytable = new Paytable();

            return kindIndex switch
            {
                0 => PaidSpin.Create($"paid-{seed}", time, 5, reels, 100, paytable),
                1 => FreeSpin.Create($"free-{seed}", time, 5, reels, 100, paytable),
                _ => throw new ArgumentOutOfRangeException(nameof(kindIndex)),
            };
        }

        public string Describe(IItem item) => _operations.Describe(item);

        public IReadOnlyList<string> DescribeAll(IItemStore store) => _operations.DescribeAll(store);

        public string Classify(IItem item) => _operations.Classify(item);

        public bool TryFindByName(IItemStore store, string name, out IItem? found) =>
            _operations.TryFindByName(store, name, out found);

        public int AddAll(IItemStore store, params IItem[] items) => _operations.AddAll(store, items);
    }
}

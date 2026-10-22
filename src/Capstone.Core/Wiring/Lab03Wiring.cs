using Capstone.Core.Search;
using SharpQuest.Contracts;

namespace Capstone.Core.Wiring
{
    public sealed class Lab03Wiring : ILab03Wiring
    {
        public IItemStore CreateStore() => new SpinCollection();

        public IItem CreateSample(int seed) => new Lab02Wiring().CreateSample(seed);

        public IReadOnlyList<IItem> Search(IItemStore store, string query) =>
            new ItemSearch().Search(store, query);
    }
}

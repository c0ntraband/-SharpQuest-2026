using Capstone.Core.Mapping;
using Capstone.Core.Models;
using SharpQuest.Contracts;

namespace Capstone.Core.Wiring
{
    public sealed class Lab02Wiring : ILab02Wiring
    {
        public IItemStore CreateStore() => new SpinCollection();

        public IItemMapper CreateMapper() => new SpinMapper();

        public IItem CreateSample(int seed)
        {
            var random = new Random(seed);
            var symbols = Enum.GetValues<Symbol>();
            var reels = new Reels(
                symbols[random.Next(symbols.Length)],
                symbols[random.Next(symbols.Length)],
                symbols[random.Next(symbols.Length)]);

            const int bet = 5;
            var payout = new Paytable().PayoutFor(reels, bet);
            var time = new DateTime(2026, 10, 1, 18, 0, 0).AddSeconds(seed * 10);

            return new Spin($"spin-{seed}", time, bet, reels, payout, creditsAfter: 100 - bet + payout);
        }
    }
}

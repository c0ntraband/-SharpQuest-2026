using Capstone.Core.Models;

namespace Capstone.Core
{
    public class SlotMachine(int seed)
    {
        private static readonly Symbol[] Symbols = Enum.GetValues<Symbol>();

        private readonly Random _random = new(seed);

        public Reels NextReels() => new(NextSymbol(), NextSymbol(), NextSymbol());

        private Symbol NextSymbol() => Symbols[_random.Next(Symbols.Length)];
    }
}

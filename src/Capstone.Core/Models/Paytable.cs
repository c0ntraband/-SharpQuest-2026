namespace Capstone.Core.Models
{
    public class Paytable : IPaytable
    {
        private readonly Dictionary<Reels, int> _multipliers = new()
        {
            [new Reels(Symbol.Cherry, Symbol.Cherry, Symbol.Cherry)] = 5,
            [new Reels(Symbol.Lemon, Symbol.Lemon, Symbol.Lemon)] = 10,
            [new Reels(Symbol.Bell, Symbol.Bell, Symbol.Bell)] = 20,
            [new Reels(Symbol.Bar, Symbol.Bar, Symbol.Bar)] = 50,
            [new Reels(Symbol.Seven, Symbol.Seven, Symbol.Seven)] = 100,
            [new Reels(Symbol.Wild, Symbol.Wild, Symbol.Wild)] = 250,
        };

        public int PayoutFor(Reels reels, int bet) =>
            _multipliers.TryGetValue(reels, out var multiplier) ? multiplier * bet : 0;
    }
}

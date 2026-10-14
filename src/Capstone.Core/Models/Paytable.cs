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
        };

        public int PayoutFor(Reels reels, int bet)
        {
            var line = CompletedLine(reels);
            if (line is not null) return _multipliers[line.Value] * bet;

            return CherriesWithWilds(reels) >= 2 ? 2 * bet : 0;
        }

        private static Reels? CompletedLine(Reels reels)
        {
            var shown = new[] { reels.Left, reels.Middle, reels.Right }
                .Where(s => s != Symbol.Wild)
                .Distinct()
                .ToList();

            return shown.Count switch
            {
                0 => new Reels(Symbol.Seven, Symbol.Seven, Symbol.Seven),
                1 => new Reels(shown[0], shown[0], shown[0]),
                _ => null,
            };
        }

        private static int CherriesWithWilds(Reels reels) =>
            new[] { reels.Left, reels.Middle, reels.Right }.Count(s => s is Symbol.Cherry or Symbol.Wild);
    }
}

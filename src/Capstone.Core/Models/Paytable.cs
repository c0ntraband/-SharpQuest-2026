namespace Capstone.Core.Models
{
    public class Paytable : IPaytable
    {
        private const int CherryPairMultiplier = 2;

        private static readonly Dictionary<Symbol, int> LineMultipliers = new()
        {
            [Symbol.Cherry] = 5,
            [Symbol.Lemon] = 10,
            [Symbol.Bell] = 20,
            [Symbol.Bar] = 50,
            [Symbol.Seven] = 100,
        };

        public int PayoutFor(Reels reels, int bet)
        {
            var line = CompletedLine(reels);
            if (line is not null) return LineMultipliers[line.Value] * bet;

            return CherriesWithWilds(reels) >= 2 ? CherryPairMultiplier * bet : 0;
        }

        private static Symbol? CompletedLine(Reels reels)
        {
            var shown = new[] { reels.Left, reels.Middle, reels.Right }
                .Where(s => s != Symbol.Wild)
                .Distinct()
                .ToList();

            return shown.Count switch
            {
                0 => Symbol.Seven,
                1 => shown[0],
                _ => null,
            };
        }

        private static int CherriesWithWilds(Reels reels) =>
            new[] { reels.Left, reels.Middle, reels.Right }.Count(s => s is Symbol.Cherry or Symbol.Wild);
    }
}

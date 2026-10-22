using static Capstone.Core.Models.Symbol;

namespace Capstone.Core.Models
{
    public class Paytable : IPaytable
    {
        public int PayoutFor(Reels reels, int bet)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bet);

            var multiplier = reels switch
            {
                (Seven or Wild, Seven or Wild, Seven or Wild) => 100,
                (Bar or Wild, Bar or Wild, Bar or Wild) => 50,
                (Bell or Wild, Bell or Wild, Bell or Wild) => 20,
                (Lemon or Wild, Lemon or Wild, Lemon or Wild) => 10,
                (Cherry or Wild, Cherry or Wild, Cherry or Wild) => 5,
                (Cherry or Wild, Cherry or Wild, _)
                    or (Cherry or Wild, _, Cherry or Wild)
                    or (_, Cherry or Wild, Cherry or Wild) => 2,
                _ => 0,
            };

            return multiplier * bet;
        }
    }
}

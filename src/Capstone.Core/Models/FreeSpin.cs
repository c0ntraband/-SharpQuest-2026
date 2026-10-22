namespace Capstone.Core.Models
{
    public sealed class FreeSpin : Spin
    {
        public const int BonusMultiplier = 2;

        public override int Cost => 0;

        public FreeSpin(string id, DateTime time, int bet, Reels reels, int payout, int creditsAfter)
            : base(id, time, bet, reels, payout, creditsAfter)
        {
        }

        public static FreeSpin Create(string id, DateTime time, int bet, Reels reels,
            int creditsBefore, IPaytable paytable)
        {
            ArgumentNullException.ThrowIfNull(paytable);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bet);
            ArgumentOutOfRangeException.ThrowIfNegative(creditsBefore);

            // Bet is the reference stake for winnings; it is never deducted.
            var payout = checked(paytable.PayoutFor(reels, bet) * BonusMultiplier);
            return new FreeSpin(id, time, bet, reels, payout, checked(creditsBefore + payout));
        }

        public override string Describe() =>
            $"Free spin ({BonusMultiplier}x payout): {Reels} — cost {Cost}, reference bet {Bet}, won {Payout}, balance {CreditsAfter}";
    }
}

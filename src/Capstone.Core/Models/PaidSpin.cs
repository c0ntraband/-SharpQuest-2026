namespace Capstone.Core.Models
{
    public sealed class PaidSpin : Spin
    {
        public override int Cost => Bet;

        public PaidSpin(string id, DateTime time, int bet, Reels reels, int payout, int creditsAfter)
            : base(id, time, bet, reels, payout, creditsAfter)
        {
        }

        public static PaidSpin Create(string id, DateTime time, int bet, Reels reels,
            int creditsBefore, IPaytable paytable)
        {
            ArgumentNullException.ThrowIfNull(paytable);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bet);
            ArgumentOutOfRangeException.ThrowIfNegative(creditsBefore);
            if (bet > creditsBefore)
                throw new InvalidOperationException("The bet is above the balance.");

            var payout = paytable.PayoutFor(reels, bet);
            return new PaidSpin(id, time, bet, reels, payout, checked(creditsBefore - bet + payout));
        }

        public override string Describe() =>
            $"Paid spin: {Reels} — cost {Cost}, won {Payout}, balance {CreditsAfter}";
    }
}

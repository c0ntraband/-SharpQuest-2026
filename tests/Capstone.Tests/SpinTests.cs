using Capstone.Core.Models;

namespace Capstone.Tests;

public class SpinTests
{
    private static readonly DateTime Time = new(2026, 10, 22, 8, 30, 0);
    private static readonly Reels CherryPair = new(Symbol.Cherry, Symbol.Cherry, Symbol.Bar);
    private readonly Paytable _paytable = new();

    [Fact]
    public void Descriptions_add_kind_specific_behaviour_beyond_the_display_name()
    {
        Spin paid = PaidSpin.Create("paid-1", Time, 5, CherryPair, 100, _paytable);
        Spin free = FreeSpin.Create("free-1", Time, 5, CherryPair, 100, _paytable);

        Assert.NotEqual(paid.DisplayName, paid.Describe());
        Assert.NotEqual(free.DisplayName, free.Describe());
        Assert.Contains("cost 5", paid.Describe());
        Assert.Contains("2x payout", free.Describe());
    }

    [Fact]
    public void A_paid_spin_deducts_the_bet_and_collects_the_regular_payout()
    {
        var spin = PaidSpin.Create("paid-1", Time, 5, CherryPair, 100, _paytable);

        Assert.Equal(5, spin.Cost);
        Assert.Equal(10, spin.Payout);
        Assert.Equal(105, spin.CreditsAfter);
        Assert.True(spin.IsWin);
    }

    [Fact]
    public void A_free_spin_costs_nothing_and_doubles_the_regular_payout()
    {
        var spin = FreeSpin.Create("free-1", Time, 5, CherryPair, 100, _paytable);

        Assert.Equal(0, spin.Cost);
        Assert.Equal(20, spin.Payout);
        Assert.Equal(120, spin.CreditsAfter);
    }

    [Fact]
    public void A_free_spin_can_win_with_an_empty_balance()
    {
        var spin = FreeSpin.Create("free-1", Time, 5, CherryPair, 0, _paytable);

        Assert.Equal(20, spin.CreditsAfter);
    }

    [Fact]
    public void A_losing_free_spin_leaves_the_balance_unchanged()
    {
        var reels = new Reels(Symbol.Cherry, Symbol.Lemon, Symbol.Bell);
        var spin = FreeSpin.Create("free-1", Time, 5, reels, 0, _paytable);

        Assert.Equal(0, spin.Cost);
        Assert.Equal(0, spin.Payout);
        Assert.Equal(0, spin.CreditsAfter);
        Assert.False(spin.IsWin);
    }

    [Fact]
    public void A_paid_spin_requires_enough_credits()
    {
        Assert.Throws<InvalidOperationException>(() =>
            PaidSpin.Create("paid-1", Time, 5, CherryPair, 4, _paytable));
    }

    [Fact]
    public void Mixed_spins_use_their_own_cost_and_description_through_the_base_type()
    {
        Spin[] spins =
        [
            PaidSpin.Create("paid-1", Time, 5, CherryPair, 100, _paytable),
            FreeSpin.Create("free-1", Time, 5, CherryPair, 105, _paytable),
        ];
        var session = new Session("Ada", 100, Time);
        var descriptions = new List<string>();

        foreach (var spin in spins)
        {
            if (spin.Cost > 0) session.PlaceBet(spin.Cost);
            session.Collect(spin.Payout);
            descriptions.Add(spin.Describe());
            Assert.Equal(spin.CreditsAfter, session.Balance);
        }

        Assert.Equal(125, session.Balance);
        Assert.StartsWith("Paid spin:", descriptions[0]);
        Assert.StartsWith("Free spin (2x payout):", descriptions[1]);
    }
}

using Capstone.Core.Models;

namespace Capstone.Tests;

public class PaytableTests
{
    private readonly Paytable _table = new();

    [Fact]
    public void Three_sevens_pay_the_jackpot()
    {
        var reels = new Reels(Symbol.Seven, Symbol.Seven, Symbol.Seven);

        Assert.Equal(500, _table.PayoutFor(reels, 5));
    }

    [Fact]
    public void Two_cherries_pay_something()
    {
        var reels = new Reels(Symbol.Cherry, Symbol.Cherry, Symbol.Bar);

        Assert.Equal(10, _table.PayoutFor(reels, 5));
    }

    [Fact]
    public void A_wild_completes_a_line()
    {
        var reels = new Reels(Symbol.Bar, Symbol.Bar, Symbol.Wild);

        Assert.Equal(100, _table.PayoutFor(reels, 2));
    }

    [Fact]
    public void A_non_winning_line_pays_nothing()
    {
        var reels = new Reels(Symbol.Cherry, Symbol.Lemon, Symbol.Bell);

        Assert.Equal(0, _table.PayoutFor(reels, 5));
    }
}

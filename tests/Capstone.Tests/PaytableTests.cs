using Capstone.Core.Models;

namespace Capstone.Tests;

public class PaytableTests
{
    private readonly Paytable _table = new();

    [Theory]
    [InlineData(Symbol.Cherry, 5)]
    [InlineData(Symbol.Lemon, 10)]
    [InlineData(Symbol.Bell, 20)]
    [InlineData(Symbol.Bar, 50)]
    [InlineData(Symbol.Seven, 100)]
    [InlineData(Symbol.Wild, 100)]
    public void Matching_lines_use_their_multiplier(Symbol symbol, int multiplier)
    {
        Assert.Equal(3 * multiplier, _table.PayoutFor(new Reels(symbol, symbol, symbol), 3));
    }

    [Theory]
    [InlineData(Symbol.Wild, Symbol.Bell, Symbol.Bell, 20)]
    [InlineData(Symbol.Bell, Symbol.Wild, Symbol.Bell, 20)]
    [InlineData(Symbol.Bell, Symbol.Bell, Symbol.Wild, 20)]
    [InlineData(Symbol.Wild, Symbol.Wild, Symbol.Bar, 50)]
    [InlineData(Symbol.Wild, Symbol.Bar, Symbol.Wild, 50)]
    [InlineData(Symbol.Bar, Symbol.Wild, Symbol.Wild, 50)]
    [InlineData(Symbol.Cherry, Symbol.Cherry, Symbol.Bar, 2)]
    [InlineData(Symbol.Cherry, Symbol.Bar, Symbol.Cherry, 2)]
    [InlineData(Symbol.Bar, Symbol.Cherry, Symbol.Cherry, 2)]
    [InlineData(Symbol.Cherry, Symbol.Wild, Symbol.Bar, 2)]
    [InlineData(Symbol.Wild, Symbol.Bar, Symbol.Cherry, 2)]
    [InlineData(Symbol.Bar, Symbol.Cherry, Symbol.Wild, 2)]
    [InlineData(Symbol.Wild, Symbol.Lemon, Symbol.Bar, 0)]
    public void Wilds_and_cherry_pairs_work_in_different_positions(
        Symbol left, Symbol middle, Symbol right, int multiplier)
    {
        Assert.Equal(4 * multiplier, _table.PayoutFor(new Reels(left, middle, right), 4));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void A_non_positive_bet_is_rejected(int bet)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _table.PayoutFor(new Reels(Symbol.Seven, Symbol.Seven, Symbol.Seven), bet));
    }

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

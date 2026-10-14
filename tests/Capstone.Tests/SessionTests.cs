using Capstone.Core.Models;

namespace Capstone.Tests;

public class SessionTests
{
    [Fact]
    public void A_bet_above_the_balance_is_refused()
    {
        var session = new Session("Ada", 50, new DateTime(2026, 10, 14, 9, 0, 0));

        Assert.Throws<InvalidOperationException>(() => session.PlaceBet(51));
        Assert.Equal(50, session.Balance);
    }

    [Fact]
    public void A_win_is_added_to_the_balance()
    {
        var session = new Session("Ada", 50, new DateTime(2026, 10, 14, 9, 0, 0));

        session.PlaceBet(10);
        session.Collect(25);

        Assert.Equal(65, session.Balance);
    }
}

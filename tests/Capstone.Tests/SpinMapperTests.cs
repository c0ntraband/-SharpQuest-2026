using Capstone.Core.Mapping;
using Capstone.Core.Models;

namespace Capstone.Tests;

public class SpinMapperTests
{
    private readonly SpinMapper _mapper = new();

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Round_trip_preserves_the_spin_kind_and_result(bool free)
    {
        var time = new DateTime(2026, 10, 22, 8, 30, 0);
        var reels = new Reels(Symbol.Cherry, Symbol.Cherry, Symbol.Bar);
        Spin original = free
            ? FreeSpin.Create("free-1", time, 5, reels, 100, new Paytable())
            : PaidSpin.Create("paid-1", time, 5, reels, 100, new Paytable());

        var copy = Assert.IsAssignableFrom<Spin>(_mapper.FromJson(_mapper.ToJson(original)));

        Assert.Equal(original.GetType(), copy.GetType());
        Assert.Equal(original.Id, copy.Id);
        Assert.Equal(original.Time, copy.Time);
        Assert.Equal(original.Bet, copy.Bet);
        Assert.Equal(original.Reels, copy.Reels);
        Assert.Equal(original.Cost, copy.Cost);
        Assert.Equal(original.Payout, copy.Payout);
        Assert.Equal(original.CreditsAfter, copy.CreditsAfter);
        Assert.Equal(original.DisplayName, copy.DisplayName);
    }

    [Fact]
    public void Older_json_without_a_kind_is_read_as_a_paid_spin()
    {
        const string json = """
            {"Id":"old-1","Time":"2026-10-14T09:00:00","Bet":5,
             "Left":"Seven","Middle":"Seven","Right":"Seven",
             "Payout":500,"CreditsAfter":595}
            """;

        var spin = Assert.IsType<PaidSpin>(_mapper.FromJson(json));

        Assert.Equal(5, spin.Cost);
        Assert.Equal(500, spin.Payout);
        Assert.Equal(595, spin.CreditsAfter);
    }

    [Fact]
    public void An_unknown_kind_is_rejected()
    {
        const string json = """
            {"Id":"unknown-1","Time":"2026-10-22T09:00:00","Bet":5,
             "Left":"Seven","Middle":"Seven","Right":"Seven",
             "Payout":500,"CreditsAfter":595,"Kind":"bonus"}
            """;

        Assert.Null(_mapper.FromJson(json));
    }
}

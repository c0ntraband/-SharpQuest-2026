using Capstone.Core;
using Capstone.Core.Models;
using Capstone.Core.Search;

namespace Capstone.Tests;

public class ItemSearchTests
{
    [Fact]
    public void Search_finds_both_kinds_by_reordered_words_and_preserves_store_order()
    {
        var store = new SpinCollection();
        var time = new DateTime(2026, 10, 22, 8, 30, 0);
        var reels = new Reels(Symbol.Cherry, Symbol.Cherry, Symbol.Bar);
        var paytable = new Paytable();
        store.Add(FreeSpin.Create("free-1", time, 5, reels, 100, paytable));
        store.Add(PaidSpin.Create("paid-1", time, 5, reels, 100, paytable));
        store.Add(PaidSpin.Create("paid-2", time, 5,
            new Reels(Symbol.Bell, Symbol.Bell, Symbol.Bell), 100, paytable));

        var result = new ItemSearch().Search(store, "BAR CHÉRRY");

        Assert.Equal(new[] { "free-1", "paid-1" }, result.Select(item => item.Id));
        Assert.Equal(3, store.Count);
    }

    [Fact]
    public void Search_rejects_a_null_query()
    {
        Assert.Throws<ArgumentNullException>(() => new ItemSearch().Search(new SpinCollection(), null!));
    }
}

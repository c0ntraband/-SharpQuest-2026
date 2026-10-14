using Capstone.Core;
using Capstone.Core.Models;

namespace Capstone.Tests;

public class SlotMachineTests
{
    [Fact]
    public void The_same_seed_gives_the_same_spins()
    {
        var first = new SlotMachine(42);
        var second = new SlotMachine(42);

        var firstRun = Enumerable.Range(0, 20).Select(_ => first.NextReels()).ToList();
        var secondRun = Enumerable.Range(0, 20).Select(_ => second.NextReels()).ToList();

        Assert.Equal(firstRun, secondRun);
    }
}

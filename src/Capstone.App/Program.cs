using System.Text;
using Capstone.Core;
using Capstone.Core.Models;

Console.OutputEncoding = Encoding.UTF8;

var session = new Session("Ada", startingCredits: 100, DateTime.Now);
var machine = new SlotMachine(seed: 7);
var paytable = new Paytable();
var spins = new SpinCollection();

const int bet = 5;
for (var i = 1; i <= 10; i++)
{
    var reels = machine.NextReels();
    Spin spin = i % 3 == 0
        ? FreeSpin.Create($"spin-{i}", DateTime.Now, bet, reels, session.Balance, paytable)
        : PaidSpin.Create($"spin-{i}", DateTime.Now, bet, reels, session.Balance, paytable);

    if (spin.Cost > 0) session.PlaceBet(spin.Cost);
    session.Collect(spin.Payout);

    spins.Add(spin);
    Console.WriteLine($"{spin.Id}: {spin.Describe()}");
}

try
{
    session.PlaceBet(session.Balance + 1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Refused: {ex.Message}");
}

Console.WriteLine($"Balance after {spins.Count} spins: {session.Balance} credits");

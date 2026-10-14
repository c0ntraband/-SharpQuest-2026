using Capstone.Core.Models;

namespace Capstone.Core.Mapping
{
    public record SpinDto(
        string? Id,
        DateTime? Time,
        int? Bet,
        Symbol? Left,
        Symbol? Middle,
        Symbol? Right,
        int? Payout,
        int? CreditsAfter);
}

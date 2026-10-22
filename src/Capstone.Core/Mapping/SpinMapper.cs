using System.Text.Json;
using System.Text.Json.Serialization;
using Capstone.Core.Models;
using SharpQuest.Contracts;

namespace Capstone.Core.Mapping
{
    public class SpinMapper : IItemMapper
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            Converters = { new JsonStringEnumConverter() },
        };

        public string ToJson(IItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (item is not Spin spin)
                throw new ArgumentException($"SpinMapper only maps spins, not {item.GetType().Name}.", nameof(item));

            var kind = spin switch
            {
                PaidSpin => "paid",
                FreeSpin => "free",
                _ => throw new ArgumentException("Unknown spin kind.", nameof(item)),
            };

            var dto = new SpinDto(spin.Id, spin.Time, spin.Bet,
                spin.Reels.Left, spin.Reels.Middle, spin.Reels.Right,
                spin.Payout, spin.CreditsAfter, kind);
            return JsonSerializer.Serialize(dto, Options);
        }

        public IItem? FromJson(string json)
        {
            SpinDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<SpinDto>(json, Options);
            }
            catch (JsonException)
            {
                return null;
            }

            if (dto is null
                || string.IsNullOrWhiteSpace(dto.Id)
                || dto.Time is not { } time
                || dto.Bet is not { } bet
                || dto.Left is not { } left
                || dto.Middle is not { } middle
                || dto.Right is not { } right
                || dto.Payout is not { } payout
                || dto.CreditsAfter is not { } creditsAfter)
                return null;

            try
            {
                var reels = new Reels(left, middle, right);
                return dto.Kind switch
                {
                    // Older saved spins did not have a kind and were all paid.
                    null or "paid" => new PaidSpin(dto.Id, time, bet, reels, payout, creditsAfter),
                    "free" => new FreeSpin(dto.Id, time, bet, reels, payout, creditsAfter),
                    _ => null,
                };
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}

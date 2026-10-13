namespace Capstone.Core.Models
{
    public interface IPaytable
    {
        int PayoutFor(Reels reels, int bet);
    }
}

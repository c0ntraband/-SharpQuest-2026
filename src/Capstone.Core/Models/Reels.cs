namespace Capstone.Core.Models
{
    public readonly record struct Reels(Symbol Left, Symbol Middle, Symbol Right)
    {
        public bool IsThreeOfAKind => Left == Middle && Middle == Right;

        public override string ToString() => $"{Left} | {Middle} | {Right}";
    }
}

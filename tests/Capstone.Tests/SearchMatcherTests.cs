using SharpQuest.Contracts;

namespace Capstone.Tests;

public class SearchMatcherTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" \t\n ")]
    public void An_empty_query_matches_non_null_text(string query)
    {
        Assert.True(SearchMatcher.Create().Matches("Cherry spin", query));
    }

    [Theory]
    [InlineData("CHERRY", "cherry")]
    [InlineData("Cherry", "CHERRY")]
    public void Matching_ignores_case(string text, string query)
    {
        Assert.True(SearchMatcher.Create().Matches(text, query));
    }

    [Theory]
    [InlineData("Café", "cafe")]
    [InlineData("Sosea", "șosea")]
    public void Matching_ignores_accents_in_text_and_query(string text, string query)
    {
        Assert.True(SearchMatcher.Create().Matches(text, query));
    }

    [Fact]
    public void Every_query_word_must_match()
    {
        Assert.False(SearchMatcher.Create().Matches("Cherry spin", "cherry lemon"));
    }

    [Fact]
    public void Query_words_can_be_reordered()
    {
        Assert.True(SearchMatcher.Create().Matches("Cherry winning spin", "spin cherry"));
    }

    [Fact]
    public void A_query_word_can_start_inside_a_longer_word()
    {
        Assert.True(SearchMatcher.Create().Matches("Cherry", "err"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("cherry")]
    public void Null_text_never_matches(string query)
    {
        Assert.False(SearchMatcher.Create().Matches(null, query));
    }

    [Theory]
    [InlineData("Cherry")]
    [InlineData(null)]
    public void Null_query_is_a_programming_error(string? text)
    {
        Assert.Throws<ArgumentNullException>(() => SearchMatcher.Create().Matches(text, null!));
    }
}

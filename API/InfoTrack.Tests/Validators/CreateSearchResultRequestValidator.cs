using InfoTrack.Domain.Enums;
using InfoTrack.Domain.Requests;
using InfoTrack.Domain.Requests.Validators;

namespace InfoTrack.Tests.Validators;

public class CreateSearchResultRequestValidatorTests
{
    private readonly CreateSearchResultRequestValidator _createSearchResultRequestValidator;

    public CreateSearchResultRequestValidatorTests()
    {
        _createSearchResultRequestValidator = new CreateSearchResultRequestValidator();
    }

    [Fact]
    public void Validate_GivenValidRequest_ShouldBeValid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = "keywords",
            SearchTarget = "search target",
            SearchEngine = SearchEngine.Google
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.True(actual.IsValid);
    }

    [Fact]
    public void Validate_GivenKeywordsIsEmpty_ShouldBeInvalid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = string.Empty,
            SearchTarget = "search target",
            SearchEngine = SearchEngine.Google
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.False(actual.IsValid);
    }

    [Fact]
    public void Validate_GivenKeywordsIsTooLong_ShouldBeInvalid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = new string('a', 500),
            SearchTarget = "test search target",
            SearchEngine = SearchEngine.Google
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.False(actual.IsValid);
    }

    [Fact]
    public void Validate_GivenSearchTargetIsEmpty_ShouldBeInvalid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = "keywords",
            SearchTarget = string.Empty,
            SearchEngine = SearchEngine.Google
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.False(actual.IsValid);
    }

    [Fact]
    public void Validate_GivenSearchTargetTooLong_ShouldBeInvalid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = "keywords",
            SearchTarget = new string('a', 500),
            SearchEngine = SearchEngine.Google
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.False(actual.IsValid);
    }

    [Fact]
    public void Validate_GivenSearchEngineIsInvalid_ShouldBeInvalid()
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = "test keywords",
            SearchTarget = "test search target",
            SearchEngine = (SearchEngine)99
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.False(actual.IsValid);
    }

    [Theory]
    [InlineData(SearchEngine.Google)]
    [InlineData(SearchEngine.Yahoo)]
    public void Validate_GivenSearchEngineIsValid_ShouldBeValid(SearchEngine searchEngine)
    {
        var request = new CreateSearchResultRequest
        {
            Keywords = "test keywords",
            SearchTarget = "test search target",
            SearchEngine = searchEngine
        };

        var actual = _createSearchResultRequestValidator.Validate(request);

        Assert.True(actual.IsValid);
    }
}
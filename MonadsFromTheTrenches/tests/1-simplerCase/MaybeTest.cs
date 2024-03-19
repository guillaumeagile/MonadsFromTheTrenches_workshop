using FluentAssertions;
using LanguageExt;

namespace MonadsFromTheTrenches.simplerCase;

public class MaybeTest
{
    
    [Fact]
    public void Maybe_HappyPath_ProceduralWay()
    {
        // Arrange
        var mayBeValue =  Option<int>.Some(value: 1);
    
        // Assert
        mayBeValue.IsSome .Should().BeTrue();
        // don't look into the box !!
    }
    
    [Fact]
    public void Maybe_HappyPath_FunctionnalWay()
    {
        // Arrange
        var mayBeValue =  Option<int>.Some(value: 0);
    
        // Assert
        mayBeValue.IfSome(f: value => value.Should().Be(expected: 0));

        mayBeValue.Match(
            Some: value => value.Should().Be(expected: 0), 
            None: () => Assert.Fail(message: "no no no"));
    }
    
    // and now... with Cat
}
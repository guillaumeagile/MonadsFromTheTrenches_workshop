using FluentAssertions;
using LanguageExt;

namespace MonadsFromTheTrenches.tests._1_simplerCase;

public class MaybeTest
{
    
    [Fact]
    public void Maybe_HappyPath_ProceduralWay()
    {
        // Arrange
        var mayBeValue =  Option<int>.None;
    
        // Assert
       
        mayBeValue.IfSome( x => Assert.Fail("booo") );
        
        // don't look into the box !!
    }
    
    [Fact]
    public void Maybe_HappyPath_FunctionnalWay()
    {
        // Arrange
        var mayBeValue =  Option<int?>.Some(value: 0);
    
        // Assert
       // mayBeValue.IfSome(f: value => foo());

       _ = mayBeValue.Match(
            Some: value =>  value.Should().Be(0), 
            None: () => Assert.Fail("boo"));
    }

    private Action foo()
    {
        throw new NotImplementedException();
    }

    // and now... with Cat
}
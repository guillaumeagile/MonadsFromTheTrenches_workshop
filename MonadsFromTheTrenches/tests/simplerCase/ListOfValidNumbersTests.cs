using System.Collections.Immutable;
using FluentAssertions;

namespace MonadsFromTheTrenches.simplerCase;

public class ListOfValidNumbersTests
{
    private ImmutableList<string> thatList = ImmutableList<string>.Empty;
    
    [Fact]
    public void ReadOneMovieFromRawString_HappyPath()
    {
        // Arrange
        var inputList =  thatList.Add("0").Add("1").Add("2");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
         
        
        // Assert
        expected.Count().Should().Be(3);
       var first = expected.First();
       first.IsSome.Should().BeTrue();
       
       //first.Value.Should().Be(expected: 0);  <== this is not working 
       first.IfSome(value => value.Should().Be(expected: 0));
       

        expected.Last().IfSome(x => x.Should().Be(2));
      
    }
    
    [Fact]
    public void ReadOneMovieFromRawString_UnHappyPath()
    {
        // Arrange
        var inputList =  thatList.Add(null).Add("2").Add("3");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
         
        
        // Assert
        expected.Count().Should().Be(3);
        expected.Last().IfSome(x => x.Should().Be(3));
        expected.First().IfSome(x => x.Should().Be(1));
     
        
    }
}
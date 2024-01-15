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
         ImmutableList<Int32> expected = sut.ClassicTransform(inputList);
         
         // Assert
         expected.Count().Should().Be(3);
         expected.First().Should().Be(0);
         expected.Last().Should().Be(2);
    }
    
    [Fact]
    public void ReadOneMovieFromRawString_UnHappyPath()
    {
        // Arrange
        var inputList =  thatList.Add(null).Add("2").Add("3");
        var sut = new TransformInputToListOfNumbers();

        // Act
        ImmutableList<object> expected = sut.MonadicTransform(inputList);
         
        // Assert
        expected.Count().Should().Be(3);
        expected.Last().Should().Be(2);
        
        expected.First().Should().NotBe(1);
        //but what it should be ??
        // null : forbidden, because it is not a number
        // exception : forbidden, because it is not a number, and we want to continue the process seamlessly
        // primitive obsession is one key that works, but require some effort to be done
        
        //what about Monads ?
        
    }
}
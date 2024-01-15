using FluentAssertions;

namespace MonadsFromTheTrenches;

public class UnitTestObjectCalisthenics
{        // on veux lire un fichier CSV dans lequel il y a des dates
         
         // dans une des colonnes, ily a peut une valeur vide
         
         // et peut etre aussi des valeurs invalides
    [Fact]
    public void ReadOneMovieFromRawString_HappyPath()
    {
        // Arrange
        // 
        IMovieReader sut = new FakeReader(oneLine: new List<string> { "Batman Dark Night", "5", "2008-07-18" });

        // Act
        IEnumerable<MovieReview>  result = sut.ReadMovies();

        // Assert
       result.Count().Should().Be(expected: 1);
       result.First().Should().Be(expected: new MovieReview(
           Title: new Title( "Batman Dark Night"),
           Rate: 5,
           releaseDate: new DateTime(year: 2008, month: 7, day: 18)));  
    }
    
    [Fact]
    public void ReadOneMovieFromRawString_UnHappyPath_MissingTitle()
    {
        // Arrange
        
        IMovieReader sut = new FakeReader(oneLine: new List<string> { null, "5", "2008-07-18" });

        // Act
        IEnumerable<MovieReview>  result = sut.ReadMovies();

        // Assert
        result.Count().Should().Be(expected: 1);
        result.First().Should().Be(expected: new MovieReview(
            Title: new MissingTitle(),
            Rate: 5,
            releaseDate: new DateTime(year: 2008, month: 7, day: 18)));  
    }
    
}
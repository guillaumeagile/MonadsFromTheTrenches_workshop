using FluentAssertions;

namespace MonadsFromTheTrenches;

public class UnitTestMoviesMonad
{   // TODO: faire juste une liste de ENtier qui peuvent être invalides ou pas, venu d'un stream type fichier ou BD   
    
    // on veux lire un fichier CSV dans lequel il y a des dates
         // dans une des colonnes, ily a peut une valeur vide
         // et peut etre aussi des valeurs invalides
    [Fact]
    public void ReadOneMovieFromRawString_HappyPath()
    {
        // Arrange
        var fakeReader = new FakeStringLineReader();
        fakeReader.Add(oneLine: new List<string> { "Batman Dark Night", "5", "2008-07-18" });
        IMovieReader sut = new TransformInputToMovieReview(fakeReader);
        // Act
        IEnumerable<MonadicMovieReview>  result = sut.ReadMoviesMondiac();
        // Assert
       result.Count().Should().Be(expected: 1);
       result.First().Should().Be(expected: new MonadicMovieReview(
           Title: LanguageExt.Option<string>.Some("Batman Dark Night"),
           Rate: 5,
           releaseDate: new DateTime(year: 2008, month: 7, day: 18)));  
    }
    
    [Fact]
    public void ReadOneMovieFromRawString_UnHappyPath_MissingTitle()
    {
        // Arrange
        var fakeReader = new FakeStringLineReader();
        fakeReader.Add(oneLine: new List<string> { null, "5", "2008-07-18" });
        IMovieReader sut = new TransformInputToMovieReview(fakeReader);
        // Act
        IEnumerable<MonadicMovieReview>  result = sut.ReadMoviesMondiac();
        // Assert
        result.Count().Should().Be(expected: 1);
        result.First().Should().Be(expected: new MonadicMovieReview(
            Title: LanguageExt.Option<string>.None,
            Rate: 5,
            releaseDate: new DateTime(year: 2008, month: 7, day: 18)));  
    }
    
}


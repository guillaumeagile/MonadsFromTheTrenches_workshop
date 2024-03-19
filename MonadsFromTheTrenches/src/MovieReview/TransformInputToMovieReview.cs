namespace MonadsFromTheTrenches;

public class TransformInputToMovieReview(IStringLineReader reader) : IMovieReader
{
    public IEnumerable<MovieReview> ReadMovies()
    {
        var result = new List<MovieReview>();
        var firstLineFromFile = reader.Read().FirstOrDefault();

        ITitle title =  (firstLineFromFile.ElementAt(0) == null) ? new MissingTitle():  new Title(firstLineFromFile.ElementAt(0));
        
        var movie = new MovieReview(
            Title: title,
            Rate: int.Parse(s: firstLineFromFile.ElementAt(1)),
            releaseDate: DateTime.Parse(s: firstLineFromFile.ElementAt(2)));
        
        result.Add( movie);
        return result;
    }

    public IEnumerable<MonadicMovieReview> ReadMoviesMondiac()
    {
        throw new NotImplementedException();
    }
}
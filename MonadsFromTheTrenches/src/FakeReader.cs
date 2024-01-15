namespace MonadsFromTheTrenches;

public class FakeReader : IMovieReader
{
    private List<List<string>> fileContent = new();
    public FakeReader(List<string> oneLine)
    {
        fileContent.Add(item: oneLine);
    }

    public IEnumerable<MovieReview> ReadMovies()
    {
        var result = new List<MovieReview>();
        var firstLineFromFile = fileContent.FirstOrDefault();


        ITitle title =  (firstLineFromFile[index: 0] == null) ? new MissingTitle():  new Title(firstLineFromFile[index: 0]);
        
        var movie = new MovieReview(
            Title: title,
            Rate: int.Parse(s: firstLineFromFile[index: 1]),
            releaseDate: DateTime.Parse(s: firstLineFromFile[index: 2]));
        
        result.Add( movie);
        
        return result;
    }

    public IEnumerable<MonadicMovieReview> ReadMoviesMondiac()
    {
        throw new NotImplementedException();
    }
}
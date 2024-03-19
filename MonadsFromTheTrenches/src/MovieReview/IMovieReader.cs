namespace MonadsFromTheTrenches;

public interface IMovieReader
{
    IEnumerable<MovieReview> ReadMovies();
    IEnumerable<MonadicMovieReview> ReadMoviesMondiac();
}
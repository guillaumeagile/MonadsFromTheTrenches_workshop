using LanguageExt;

namespace MonadsFromTheTrenches;

public record MonadicMovieReview(Option<string> Title, int Rate, DateTime releaseDate);

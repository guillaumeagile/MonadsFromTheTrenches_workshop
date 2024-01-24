using System.Collections.Immutable;
using LanguageExt;

namespace MonadsFromTheTrenches.simplerCase;

public class TransformInputToListOfNumbers
{
    public ImmutableList<int> ClassicTransform(ImmutableList<string> thatList)
    {
        throw new NotImplementedException();
    }

    public ImmutableList<Option<int>> MonadicTransform(ImmutableList<string> thatList)
    {
        return thatList
            .Select(item =>
                    int.TryParse(item, out var value) ? Option<int>.Some(value) : Option<int>.None)
            .ToImmutableList();
    }
}
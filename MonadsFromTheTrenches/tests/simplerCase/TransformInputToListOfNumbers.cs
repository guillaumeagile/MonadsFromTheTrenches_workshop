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
        var temp = new List<Option<int>>();

        foreach (var item in thatList)
        {
            var result = int.TryParse(item, out var value);
            if (result)
                temp.Add(Option<int>.Some(value));
            else
                temp.Add(Option<int>.None);
        }

        // thatList.Select( str =>  procedure(str))
        return temp.ToImmutableList();
    }
}
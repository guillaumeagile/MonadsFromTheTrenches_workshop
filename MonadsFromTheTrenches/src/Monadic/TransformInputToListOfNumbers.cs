using System.Collections.Immutable;
using LanguageExt;
using static LanguageExt.Prelude;

namespace MonadsFromTheTrenches.simplerCase;

public class TransformInputToListOfNumbers
{
    public ImmutableList<int> ClassicTransform(ImmutableList<string> thatList)
    {
        throw new NotImplementedException();
    }

    public ImmutableList<Option<int>> MonadicTransform0(ImmutableList<string> thatList)
    {
        return thatList
            .Select(transformStringIntoOptionOfInt)
            .ToImmutableList();
    }
    
    public ImmutableList<Option<int>> MonadicTransform(ImmutableList<string> thatList)
    {
       var seq =  thatList.ToSeq();
       var res =  seq.Map(transformStringIntoOptionOfInt);
       return res.ToImmutableList();
    }
    // TODO: on pourra  faire un exemple de Closure et Currying, peut être plus tard
    //  

    private static Option<int> transformStringIntoOptionOfInt(string item)
    {
        return int.TryParse(item, out var value) ? Option<int>.Some(value) : Option<int>.None;
    }
    // DISCUSSION: Make Illegal States Unrepresentable!
    // Always Valid Pattern
}
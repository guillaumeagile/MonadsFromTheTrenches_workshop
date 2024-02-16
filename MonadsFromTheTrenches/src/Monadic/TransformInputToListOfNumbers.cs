using System.Collections.Immutable;
using LanguageExt;
using LanguageExt.Common;
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
    
   public static Either<OurError, int> TransformStringToEitherInt(string input)
    {
        if (string.IsNullOrEmpty(input))
            return Either<OurError, int>.Left(new OurError(KindOfError.EMPTY_STRING, input));
        
        return int.TryParse(input, out var value)? Either<OurError, int>.Right(value) : Either<OurError, int>.Left(new OurError(KindOfError.INVALID_NUMBER, input));
    }
    
    
    public ImmutableList<Either<OurError, int>> MonadicTransformToEither(ImmutableList<string> inputList)
    {
        return inputList.Map(TransformStringToEitherInt).ToImmutableList();
    }
}

public record OurError(KindOfError kindOfError, string OriginalInput);


public enum KindOfError
{
    EMPTY_STRING,
    INVALID_NUMBER,
    NEGATIVE_ERROR
}
using System.Collections.Immutable;
using LanguageExt;
using LanguageExt.Common;
using MonadsFromTheTrenches.Catz;
using static LanguageExt.Prelude;

namespace MonadsFromTheTrenches;

public static class TakeCareOfCats
{
    public static ImmutableList<Cat> FeedCats(ImmutableList<Cat> inputList, int i)
    {
        throw new NotImplementedException();
    }
 

    public static ImmutableList<Either<Error, Cat>> SchrödingerCats(this ImmutableList<Cat> inputList) => throw new NotImplementedException();

    public static IEnumerable<Either<Error, Cat>> FeedSchrödingerCats(this ImmutableList<Either<Error, Cat>> inputList, int i)
    {
        return inputList;
    }

    
    
    

    // ------------- soluces -----------------
    
    public static IEnumerable<Either<Error, Cat>> FeedSchrödingerCats_Sol(this ImmutableList<Either<Error, Cat>> inputList, int i, Func<Cat, Cat> CatTakesWeightPlus1 )
    {
        var res1 = inputList.Map(cat => cat.Map(c => new Cat(c.Name, c.Age, c.Weight + i)));
        
        IEnumerable<Either<Error, Cat>> res2 = from cat in inputList
            select cat.Map(c => CatTakesWeight( i, c));

        var seq = inputList.ToSeq();
            
        var res3 = seq.BiMapT(
            cat =>  cat with { Weight = cat.Weight + i},
            e => e);
        
        var res4 = seq.BiMapT(
            cat =>  CatTakesWeightPlus1,
            e => e);
        
        return res3;
    }

   

    private static Cat CatTakesWeight(int i, Cat cat)
    {
        return cat with { Weight = cat.Weight + i};
    }
}
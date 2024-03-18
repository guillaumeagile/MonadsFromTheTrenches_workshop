using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.DataTypes.Serialisation;
using Microsoft.AspNetCore.Mvc;
using MonadsFromTheTrenches.Catz;
using MonadsFromTheTrenches.simplerCase;

namespace MonadsFromTheTrenches;

public class CatzTests
{
    private ImmutableList<Cat> thatList = ImmutableList<Cat>.Empty;

    [Fact]
    public void ListOfNiceCats()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat(Name : "Fluffy", Age : 8, Weight: 10))
            .Add(new Cat(Name : "Mittens", Age : 5, Weight: 7))
            .Add(new Cat(Name : "Garfield", Age : 7, Weight: 18));
        
        // Act
        var expected = TakeCareOfCats.FeedCats(inputList, 1);
        
        // Assert
        expected.Count().Should().Be(3);
        expected.First().Name.Should().Be("Fluffy");
        expected.First().Weight.Should().Be(11);
        expected.Last().Name.Should().Be("Garfield");
        expected.Last().Weight.Should().Be(19);

    }
    
    [Fact]
    public void MakeListOfSchrödingerCats()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat(Name : "Fluffy", Age : 8, Weight: 10))
            .Add(new Cat(Name : "Mittens", Age : 5, Weight: 7))
            .Add(new Cat(Name : "Garfield", Age : 7, Weight: 18));
       
     
        // Act
        var expected = inputList.SchrödingerCats();
        
        // Assert
       

    }
    
    [Fact]
    public void FeedListOfSchrödingerCats()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat(Name : "Fluffy", Age : 8, Weight: 9))
            .Add(new Cat(Name : "Mittens", Age : 5, Weight: 7))
            .Add(new Cat(Name : "Garfield", Age : 7, Weight: 17));
        
        var sut = inputList.SchrödingerCats();
        
        // Act
        var expected = sut.FeedSchrödingerCats( 1);
        
        // Assert
        // tous les chats on prit +1
         expected.Sum(box => box.Fold(
             state: 0, 
             Right: (i, c) => i + c.Weight)
             )
             .Should().Be((9 + 1) + (7 + 1) +(17 + 1));

    }
    
    [Fact]
    public void FeedListOfSchrödingerCats_OnlyIfWeightInferiorStrict10_withError()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat(Name : "Fluffy", Age : 8, Weight: 10))
            .Add(new Cat(Name : "Mittens", Age : 5, Weight: 7))
            .Add(new Cat(Name : "Garfield", Age : 7, Weight: 18));
        
        var sut = inputList.SchrödingerCats();
        
        // Act
        var expected = sut.FeedSchrödingerCats( 1);
        
        // Assert
        expected.ToArr().Sum(box => box.BiFold(
                state: 0, 
                Right: (i, c) => i , 
                Left: (i, err) => i + 1))
            .Should().Be((1 + 1));

        Seq<Either<Error, Cat>> unused = expected.ToSeq();
        var k =  unused.Bind(SaveTheContentOfTheBox);
       var kk =  unused.Apply(SaveTheContentOfAllTheBoxes);

        var z = expected.ToSeq() .Map(
            box => box
                .Match(
                    cat => (ActionResult)new OkObjectResult(cat),
                    err => (ActionResult)new BadRequestObjectResult(err))
        );
        
        var t =  z.Bind(doSomething);
       var u =  z.Apply(doSomethingElse);
        
        var zz = expected.ToArr() .MapLeftT(
            err =>  (ActionResult)new BadRequestObjectResult(err))
        ;
        var zzTop = zz.Map(box => box.Map(cat => (ActionResult)new OkObjectResult(cat)));
          var finish =   zzTop.Match( x=> x, x => x); 
        
    }

    private Either<Error, Cat> SaveTheContentOfAllTheBoxes(Seq<Either<Error, Cat>> arg)
    {
        throw new NotImplementedException();
    }


    private Seq<Either<Error, Cat>> SaveTheContentOfTheBox(Either<Error, Cat> arg)
    {
        return new Seq<Either<Error, Cat>>().Add(arg);
    }

    private int doSomethingElse(Seq<ActionResult> arg)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<int> doSomething (ActionResult arg)
    {
        yield return 1;
    }
}
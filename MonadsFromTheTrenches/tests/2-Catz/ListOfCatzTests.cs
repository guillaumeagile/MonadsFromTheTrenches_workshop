using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using MonadsFromTheTrenches.Catz;

namespace MonadsFromTheTrenches;

public class ListOfCatzTests
{
    private readonly ImmutableList<Cat> thatList = ImmutableList<Cat>.Empty;

    [Fact]
    public void ListOfNiceCats()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat("Fluffy", 8, 10))
            .Add(new Cat("Mittens", 5, 7))
            .Add(new Cat("Garfield", 7, 18));

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
            .Add(new Cat("Fluffy", 8, 10))
            .Add(new Cat("Mittens", 5, 7))
            .Add(new Cat("Garfield", 7, 18));


        // Act
        var expected = inputList.SchrödingerCats();

        // Assert
    }
    
    [Fact]
    public void FeedListOfSchrödingerCats()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat("Fluffy", 8, 9))
            .Add(new Cat("Mittens", 5, 7))
            .Add(new Cat("Garfield", 7, 17));

        var sut = inputList.SchrödingerCats();

        // Act
        var expected = sut.FeedSchrödingerCats(1);

        // Assert
        // tous les chats on prit +1
        expected.Sum(box => box.Fold(
                0,
                (i, c) => i + c.Weight)
            )
            .Should().Be(9 + 1 + 7 + 1 + 17 + 1);
    }

    [Fact]
    public void FeedListOfSchrödingerCats_OnlyIfWeightInferiorStrict10_withError()
    {
        // Arrange
        var inputList = thatList
            .Add(new Cat("Fluffy", 8, 10))
            .Add(new Cat("Mittens", 5, 7))
            .Add(new Cat("Garfield", 7, 18));

        var sut = inputList.SchrödingerCats();

        // Act
        var expected = sut.FeedSchrödingerCats(1);

        // Assert
        expected.ToArr().Sum(box => box.BiFold(
                0,
                (i, c) => i,
                (i, err) => i + 1))
            .Should().Be(1 + 1);

        var unused = expected.ToSeq();
        var k = unused.Bind(SaveTheContentOfTheBox);
        var kk = unused.Apply(SaveTheContentOfAllTheBoxes);

        var z = expected.ToSeq().Map(
            box => box
                .Match(
                    Right: cat => new OkObjectResult(cat),
                    Left: err => (ActionResult)new BadRequestObjectResult(err))
        );

        var t = z.Bind(doSomething);
        var u = z.Apply(doSomethingElse);

        var zz = expected.ToArr().MapLeftT(
                err => (ActionResult)new BadRequestObjectResult(err))
            ;
        var zzTop = zz.Map(box => box.Map(cat => (ActionResult)new OkObjectResult(cat)));
        
        var finished = zzTop.Match(x => x, x => x);
    }

    private Either<Error, Cat> SaveTheContentOfAllTheBoxes(Seq<Either<Error, Cat>> arg)
    {
        throw new NotImplementedException();
    }

    private Seq<Either<Error, Cat>> SaveTheContentOfTheBox(Either<Error, Cat> arg) //Bind
    {
        return new Seq<Either<Error, Cat>>().Add(arg);
    }

    private int doSomethingElse(Seq<ActionResult> arg)  //Apply
    {
        throw new NotImplementedException();
    }

    private IEnumerable<int> doSomething(ActionResult arg) //Bind
    {
        yield return 1;
    }
}

/*
 *  private Either<Error, Cat> GetsOlderIfNotTooOld(Cat arg) //Bind
   {
   if (arg.Age > 7) return Either<Error, Cat>.Left(new MyError("TOO_OLD"));
   return arg with { Age = arg.Age + 1 };
   }
   
   private Either<Error, Dog> OldCatBecomesDog(Cat arg) //Bind
   {
   if (arg.Age > 7) return Either<Error, Dog>.Left(new MyError("TOO_OLD_TO_BECOME_DOG"));
   return new Dog( Name : arg.Name + " woof!");
   }
 * 
 */

public record MyError : Error
{
    public MyError(string reason) : base(reason)
    {
    }

    public override string Message { get; }
    public override bool IsExceptional { get; }
    public override bool IsExpected { get; }


    public override bool Is<E>()
    {
        throw new NotImplementedException();
    }

    public override ErrorException ToErrorException()
    {
        throw new NotImplementedException();
    }
}
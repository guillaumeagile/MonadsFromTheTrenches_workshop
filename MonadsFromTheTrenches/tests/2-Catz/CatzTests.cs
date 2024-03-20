using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using MonadsFromTheTrenches.Catz;

namespace MonadsFromTheTrenches;

public class CatzTests
{
    private readonly ImmutableList<Cat> thatList = ImmutableList<Cat>.Empty;

   

    [Fact]
    public void MakeOneSchrödingerCats()
    {
            // Arrange
            var box = Either<Error, Cat>.Right(new Cat("Fluffy", 8, 10));

        // now try to make it left/wrong
        
        box.IsLeft.Should().BeFalse();
        
        box.Match ( Left: err => Assert.Fail("booo"),
            Right:  c => c.Age.Should().Be(8) );

        // !! 🤪 !! for the rest of the demo, we will alternate between right and left


        
        // Act
        //  //Validate, Extract, Transform, lift (if valid)
        var expectOlder = box.Map( c => c with { Age = c.Age + 1});
        expectOlder.IfRight( c => c.Age.Should().Be(9));
        
        // First, there is no difference between Map and Select.
        
  
        
                //Map and Bind do much the same, however they differ only in how they return the transformed result i.e. the Lift/leave phase.
                //In all cases the transformation function is run, however the requirements of the transformation's functions arguments
                //and return type have changed:

        //  //Validate, Extract, Transform+, lift
        var expected = expectOlder.Bind(GetsOlderIfNotTooOld);
        
        expected.Match ( Left: err => err.Should().Be(new MyError("TOO_OLD")),
            Right:  c => c.Age.Should().Be(10) );

         
        //Bind requires the programmer to define and provide a transformation function that will take as input the item provided to it by the Bind function,
        //but crucially it needs to transform it in such a way that the result is a new instance of the Monad
        
        //while Map, only requires the transformation function to return the transformation from TA to TB without needing to place it into a new Box<TB>.
        
      //  var expectMapped = box.Map(transformToDog);


        
        
        var expectedBindAndChange = expected.Bind(OldCatBecomesDog);
        
        expectedBindAndChange.Match ( Left: err => err.Should().Be(new MyError("sssss")),
            Right:  c => c.Name.Should().Be("Fluffy woof!") );

        
        
        
        
        // The so-called “applicative” style uses functions such as apply, 
        var expected2 = box.Apply(SorsDeLaVilainMatou);
        
        
           return;
        
        
        var expected3 = box.Apply(Shazzam);
        
        
        
        
    }
    

    private Dog transformToDog(Cat arg) //Map
    {
        throw new NotImplementedException();
    }


    private Either<Error, Cat> GetsOlderIfNotTooOld(Cat arg) //Bind
    {
        if (arg.Age > 10) return Either<Error, Cat>.Left(new MyError("TOO_OLD"));
        return arg with { Age = arg.Age + 1 };
    }
    
    private Either<Error, Dog> OldCatBecomesDog(Cat arg) //Bind
    {
       return new Dog( Name : arg.Name + " woof!");
    }

    private Cat SorsDeLaVilainMatou(Either<Error, Cat> arg) //Apply
    {
        throw new NotImplementedException();
    }
    
    private Dog Shazzam(Either<Error, Cat> arg) //Apply
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void FullProcessOneSchrödingerCats()
    {
        // Arrange
        // on a recu le chat depuis une fenetre web (DTO) et on l'a transformé comme une entité métier de l'application.
        var inputBox = Either<Error, Cat>.Right(new Cat("Fluffy", 8, 10));
        
        var httpResponse = 
        inputBox
            .Bind(validate)  //peut nous faire passer du coté gauche si erreur de validation
            .Map(transformToDTO)        
            .Bind(save) //peut nous faire passer du coté gauche si erreur d'enregistrement en base de données
            .BiMap(Left: err => new NotFoundResult(), 
                Right: x => new OkObjectResult(x));
            
         httpResponse.Match( Right: x => x, Left: x => x);  // pour sortir de la Monad et ne produire que
                                                            // un monotype: le resultat.

    }

    public Func<Cat, Either<Error, object>> validate { get; set; }
}
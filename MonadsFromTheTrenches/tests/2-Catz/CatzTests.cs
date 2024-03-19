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

        box.IsRight.Should().BeFalse();
        
        // now try to make it left/wrong
        
        box.IsLeft.Should().BeFalse();

        // !! 🤪 !! for the rest of the demo, we will alternate between right and left
        
        // Act
        //  //Validate, Extract, Transform, lift (if valid)
        var expectOlder = box.Map( c => c );
        expectOlder.IfRight( c => c.Age.Should().Be(9));
        
        // First, there is no difference between Map and Select.
        
        return;
        
                //Map and Bind do much the same, however they differ only in how they return the transformed result i.e. the Lift/leave phase.
                //In all cases the transformation function is run, however the requirements of the transformation's functions arguments
                //and return type have changed:

        //  //Validate, Extract, Transform+, lift
        var expected = box.Bind(GetsOlderIfNotTooOld);

        
        //Bind requires the programmer to define and provide a transformation function that will take as input the item provided to it by the Bind function,
        //but crucially it needs to transform it in such a way that the result is a new instance of the Monad
        
        //while Map, only requires the transformation function to return the transformation from TA to TB without needing to place it into a new Box<TB>.
        
        var expectMapped = box.Map(transformToDog);


        
        
        var expectedBindAndhange = box.Bind(OldCatBecomesDog);
        
        
        
        
        
        // The so-called “applicative” style uses functions such as apply, 
        var expected2 = box.Apply(SorsDeLaVilainMatou);
        
        
        
        
        
        var expected3 = box.Apply(Shazzam);
        
        
        
        
    }
    

    private Dog transformToDog(Cat arg) //Map
    {
        throw new NotImplementedException();
    }


    private Either<Error, Cat> GetsOlderIfNotTooOld(Cat arg) //Bind
    {
        throw new NotImplementedException();
    }
    
    private Either<Error, Dog> OldCatBecomesDog(Cat arg) //Bind
    {
        throw new NotImplementedException();
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
        // on a recu le chat depuis une fenetre web (DTO)
        var inputBox = Either<Error, Cat>.Right(new Cat("Fluffy", 8, 10));

        // var validated = inputBox.Bind(validate);
        
        // var readyToBeSaved = validated.Map(transformToDTO);
        
        // var saved = readyToBeSaved.Bind(save);
        
        // var httpResponse = saved.BiMap(transformToHttpResponse);
        
        // httpResponse.Match( Right: x => x, Left: x => x);

    }


    

}
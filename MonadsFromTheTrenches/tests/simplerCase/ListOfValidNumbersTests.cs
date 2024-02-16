using System.Collections.Immutable;
using FluentAssertions;
using LanguageExt;
using MonadsFromTheTrenches.simplerCase;
using Xunit;
using static LanguageExt.Prelude;

namespace MonadsFromTheTrenches.simplerCase;

public class ListOfValidNumbersTests
{
    private ImmutableList<string> thatList = ImmutableList<string>.Empty;
    
    [Fact]
    public void ReadFromRawString_HappyPath()
    {
        // Arrange
        var inputList =  thatList.Add("0").Add("1").Add("2");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
        
        // Assert
        expected.Count().Should().Be(3);
       var first = expected.First();
       first.IsSome.Should().BeTrue();
       
       //first.Value.Should().Be(expected: 0);  <== this is not working 
       first.IfSome(value => value.Should().Be(expected: 0));
       
       expected.Last().IfSome(x => x.Should().Be(2));
    }
    
    [Fact]
    public void ReadFromRawString_UnHappyPath()
    {
        // Arrange
        var inputList =  thatList.Add(null).Add("2").Add("3");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
         
        // Assert
        expected.Count().Should().Be(3);
        expected.First().IsNone.Should().BeTrue();
        expected.Last().IfSome(x => x.Should().Be(3));
       
    }
    
    [Fact]
    public void ReadFromRawString_UnHappyPath_emptystring()
    {
        // Arrange
        var inputList =  thatList.Add("").Add("2").Add("3");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
         
        // Assert
        expected.Count().Should().Be(3);
        expected.First().IsNone.Should().BeTrue();
        expected.Last().IfSome(x => x.Should().Be(3));

    }
    
    [Fact]
    public void ReadFromRawString_UnHappyPath_invalidNumber()
    {
        // Arrange
        var inputList =  thatList.Add("Z").Add("2").Add("3");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransform(inputList);
         
        // Assert
        expected.Count().Should().Be(3);
        expected.First().IsNone.Should().BeTrue();
        expected.Last().IfSome(x => x.Should().Be(3));
    }
    
    // TODO NEXT: que faire (de pratique) avec une liste de Monad qui contient et des NONE et des SOME ?
    //  exemple de FILTER
    
    [Fact]
    public void ReadFromRawString_HappyPath_Filter()
    {
        // Arrange
        var inputList =  thatList.Add("0").Add("1").Add("2").Add("A");
        var sut = new TransformInputToListOfNumbers();

        var afterTransform = sut.MonadicTransform(inputList);
 
        var actual = afterTransform.Somes().Filter(x => x >= 1);   //(x => x > 1);
        // interessant mais on a perdu les élements invalides (tout dépend ce qu'on veut)
        // on verra plus tard une chaine monadique intégrale
               
        // Assert
       actual.Count().Should().Be(2);
    }
    // de MAP
    // puis de BIND /FLATMAP
    // et de REDUCE
    
    
    // étape pédagogique: étudions EITHER tout seul 
   // Erreur ou Valide ?
    // on veut savoir la nature de l'erreur: chaine vide? ou chaine invalide?
    [Fact]
    public void TestTransformStringToEitherInt_SuccessRight()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("0");
        actual.IsRight.Should().BeTrue();
        actual.IfRight( x => x.Should().Be(0));      
    }
    
    [Fact]
    public void TestTransformStringToEitherInt_SuccessRight_MapNewOperation()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("0");
    
        var afterMap = actual.Map(x => x + 1);
        afterMap.IsRight.Should().BeTrue();
        afterMap.IfRight(x => x.Should().Be(1));
    }
    
    [Fact]
    public void TestTransformStringToEitherInt()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("");
        actual.IsLeft.Should().BeTrue();
        actual.IfLeft(error => error.kindOfError.Should().Be(KindOfError.EMPTY_STRING));
      
    }
    
    [Fact]
    public void TestTransformStringToEitherInt_EmptyString_MapNewFonction()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("");
     
        var afterMap = actual.Map(x => x + 1);
        
        afterMap.IsLeft.Should().BeTrue();
        afterMap.IfLeft(error => error.kindOfError.Should().Be(KindOfError.EMPTY_STRING));
    }
    
    [Fact]
    public void TestTransformStringToEitherInt_InvalidNumer()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("A");
        actual.IsLeft.Should().BeTrue();
        actual.IfLeft(error => error.kindOfError.Should().Be(KindOfError.INVALID_NUMBER));
    }
    
    [Fact]
    public void TestTransformStringToEitherInt_InvalidNumer_ChangeLeft()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("A");
         
        var afterMap = actual.BiMap(
            Right: (x) => x ,
            Left: err => new OurError(KindOfError.EMPTY_STRING, err.OriginalInput));
        
        afterMap.IsLeft.Should().BeTrue();
        afterMap.IfLeft(error => error.kindOfError.Should().Be(KindOfError.EMPTY_STRING));
    }

    
    [Fact]
    public void fromListWithIllegalTerms_To_Etiher_WithMessage_EmptyString()
    {
        // Arrange
        var inputList =  thatList.Add("").Add("2").Add("A");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransformToEither(inputList);
        
        // Assert
        expected.Count().Should().Be(3);
        expected.First().IsLeft.Should().BeTrue();
        expected.First().IfLeft(error => error.kindOfError.Should().Be(KindOfError.EMPTY_STRING));
        expected.Last().IfLeft(error => error.kindOfError.Should().Be(KindOfError.INVALID_NUMBER));
        expected.Last().IfLeft(error => error.OriginalInput.Should().Be("A"));
        expected[1].IfRight(x => x.Should().Be(2));
    }
    
    [Fact]
    public void fromListWithIllegalTerms_To_Etiher_WithMessage_EmptyString_Map()
    {
        // Arrange
        var inputList =  thatList.Add("").Add("5").Add("A");
        var sut = new TransformInputToListOfNumbers();

        // Act
        var expected = sut.MonadicTransformToEither(inputList);
        
        var afterMap = expected.Map( x => x.Map( z => z + 1));
        
        afterMap.First().IsLeft.Should().BeTrue();
        afterMap.ToList()[1].IfRight(x => x.Should().Be(6));
        afterMap.Last().IfLeft(error => error.kindOfError.Should().Be(KindOfError.INVALID_NUMBER));
        
        var afterBiMapT =expected.BiMapT(
            Right: x => x + 1,
            Left: e => e);
        // tester ce qui en ressort est interessant
        afterBiMapT.First().IsLeft.Should().BeTrue();
        afterBiMapT.ToList()[1].IfRight(x => x.Should().Be(6));
        afterBiMapT.Last().IfLeft(error => error.kindOfError.Should().Be(KindOfError.INVALID_NUMBER));
    }
    
    // TODO:   et maintenant Bind, travailler sur le cas de l'erreur , par exemple l'enregistrer
    [Fact]
    public void test_todo()
    {
        // Arrange
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("-1");

        var expected = actual.Bind(funValidateOnlyPositive);

        expected.IsLeft.Should().BeTrue();
        expected.IfLeft(err => err.kindOfError.Should().Be(KindOfError.NEGATIVE_ERROR));
    }

    private Either<OurError, int> funValidateOnlyPositive(int arg)
    {
        throw new NotImplementedException();
    }
    // TODO ++++ =  montrer la Validation 
    
    // TODO ++++ =  montrer  qu'on veut faire une operation type Log ou Print -> effet de bord

    // TODO+++: peut on passer d'une monade à une autre monade?
    

}

// on peut aussi regarder un tutoriel github
// https://github.com/stumathews/UnderstandingLanguageExt
using System.Collections.Immutable;
using FluentAssertions;
using MonadsFromTheTrenches.simplerCase;
using Xunit;
using static LanguageExt.Prelude;

namespace MonadsFromTheTrenches.simplerCase;

public class ListOfValidNumbersTests
{
    private ImmutableList<string> thatList = ImmutableList<string>.Empty;
    
    [Fact]
    public void ReadOneMovieFromRawString_HappyPath()
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
    public void ReadOneMovieFromRawString_UnHappyPath()
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
    public void ReadOneMovieFromRawString_UnHappyPath_emptystring()
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
    public void ReadOneMovieFromRawString_UnHappyPath_invalidNumber()
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
    public void ReadOneMovieFromRawString_HappyPath_Filter()
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
    
    
    // étape pédagogique: étudions EITHER
   // Erreur ou Valide ?
    // on veut savoir la nature de l'erreur: chaine vide? ou chaine invalide?
    [Fact]
    public void TestTransformStringToEitherInt_SuccessRight()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("0");
        actual.IsRight.Should().BeTrue();
        actual.IfRight( x => x.Should().Be(0));
        // TODO     // utiliser MAP sur Either et voir que Map ne s'applique à DROITE
        var afterMap = actual.Map(x => x + 1);
    }
    
    
    [Fact]
    public void TestTransformStringToEitherInt()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("");
        actual.IsLeft.Should().BeTrue();
        actual.IfLeft(error => error.kindOfError.Should().Be(KindOfError.EMPTY_STRING));
        // TODO     // utiliser MAP sur Either et voir que Map ne s'applique à DROITE
        var afterMap = actual.Map(x => x + 1);
    }
    
    [Fact]
    public void TestTransformStringToEitherInt_InvalidNumer()
    {
        var actual = TransformInputToListOfNumbers.TransformStringToEitherInt("A");
        actual.IsLeft.Should().BeTrue();
        actual.IfLeft(error => error.kindOfError.Should().Be(KindOfError.INVALID_NUMBER));
        //TODO     // utiliser MAP sur Either et voir que Map ne s'applique à DROITE
        var afterMap = actual.Map(x => x + 1);
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

        // TODO : on ne pourra écrire ceci que si on sait l'écrire avec la Monade seule (sans la liste)
        // var res = expected.Map(x => x.Map( w => w + 1));
        // tester ce qui en ressort est interessant

    }
    
    //TODO+++: peut on passer d'une monade à une autre monade?
    
    // TODO ++++ =  montrer la Validation 
}
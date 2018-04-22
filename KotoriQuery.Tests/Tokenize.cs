using System.Linq;
using KotoriQuery.Tokenize;
using Xunit;

namespace KotoriQuery.Tests
{
    public class Tokenize
    {
        [Fact]
        public void AhOhYikes()
        {
            var q = "ah,oh797,yikes";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(6, atoms.Count());
            
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[0].Type);
            Assert.Equal(AtomType.Comma, atoms.ToArray()[1].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[2].Type);
            Assert.Equal(AtomType.Comma, atoms.ToArray()[3].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[4].Type);
            Assert.Equal(AtomType.Done, atoms.ToArray()[5].Type);
        }

        [Theory]
        [InlineData("Ah,   Oh797,yikes")]
        [InlineData("Ah,   Oh_797,yikes")]
        [InlineData("Ah, Oh_797,yi__kes")]
        public void AhOhYikesWithSpace(string q)
        {
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(7, atoms.Count());
            
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[0].Type);
            Assert.Equal(AtomType.Comma, atoms.ToArray()[1].Type);
            Assert.Equal(AtomType.Spaces, atoms.ToArray()[2].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[3].Type);
            Assert.Equal(AtomType.Comma, atoms.ToArray()[4].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[5].Type);
            Assert.Equal(AtomType.Done, atoms.ToArray()[6].Type);
        }

        [Fact]
        public void DotInIdentifier()
        {
            var q = "foo.Bar.x_y_z_1";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(6, atoms.Count());
            
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[0].Type);
            Assert.Equal("foo", atoms.ToArray()[0].GetText(q));
            Assert.Equal(AtomType.Dot, atoms.ToArray()[1].Type);
            Assert.Equal(".", atoms.ToArray()[1].GetText(q));;
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[2].Type);
            Assert.Equal("Bar", atoms.ToArray()[2].GetText(q));;
            Assert.Equal(AtomType.Dot, atoms.ToArray()[3].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[4].Type);
            Assert.Equal("x_y_z_1", atoms.ToArray()[4].GetText(q));;
            Assert.Equal(AtomType.Done, atoms.ToArray()[5].Type);
        }

        [Theory]
        [InlineData("±")]
        //[InlineData("!")]
        [InlineData("?")]
        public void BadOnes(string q)
        {
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(2, atoms.Count());
            Assert.Equal(AtomType.Bad, atoms.First().Type);
        }

        [Fact]
        public void SlashesInIdentifier()
        {
            var q = "foo/Bar/x_y_z_1";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(6, atoms.Count());
            
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[0].Type);
            Assert.Equal("foo", atoms.ToArray()[0].GetText(q));
            Assert.Equal(AtomType.Slash, atoms.ToArray()[1].Type);
            Assert.Equal("/", atoms.ToArray()[1].GetText(q));;
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[2].Type);
            Assert.Equal("Bar", atoms.ToArray()[2].GetText(q));;
            Assert.Equal(AtomType.Slash, atoms.ToArray()[3].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[4].Type);
            Assert.Equal("x_y_z_1", atoms.ToArray()[4].GetText(q));;
            Assert.Equal(AtomType.Done, atoms.ToArray()[5].Type);
        }

        [Theory]
        [InlineData("foo/bar eq 123")]
        [InlineData("foo/bar eq 0")]
        [InlineData("foo/eqaul eq 0")]
        [InlineData("eq/eqaul eq 42")]
        public void ConditionEqString(string q)
        {
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(8, atoms.Count());

            Assert.Equal(AtomType.Identifier, atoms.ToArray()[0].Type);
            Assert.Equal(AtomType.Slash, atoms.ToArray()[1].Type);
            Assert.Equal(AtomType.Identifier, atoms.ToArray()[2].Type);
            Assert.Equal(AtomType.Spaces, atoms.ToArray()[3].Type);
            Assert.Equal(AtomType.Equal, atoms.ToArray()[4].Type);
            Assert.Equal(AtomType.Spaces, atoms.ToArray()[5].Type);
            Assert.Equal(AtomType.Integer, atoms.ToArray()[6].Type);
            Assert.Equal(AtomType.Done, atoms.ToArray()[7].Type);
        }
    }
}
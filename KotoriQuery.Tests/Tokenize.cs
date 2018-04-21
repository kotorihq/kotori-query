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
    }
}
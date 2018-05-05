using System.Collections.Generic;
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
            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Comma,
                AtomType.Identifier,
                AtomType.Comma,
                AtomType.Identifier,
                AtomType.Done
            }, atoms.Select(x => x.Type));
        }

        [Theory]
        [InlineData("Ah,   Oh797,yikes")]
        [InlineData("Ah,   Oh_797,yikes")]
        [InlineData("Ah, Oh_797,yi__kes")]
        public void AhOhYikesWithSpace(string q)
        {
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(7, atoms.Count());
            
            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Comma,
                AtomType.Spaces,
                AtomType.Identifier,
                AtomType.Comma,
                AtomType.Identifier,
                AtomType.Done
            }, atoms.Select(x => x.Type));
        }

        [Fact]
        public void DotInIdentifier()
        {
            var q = "foo.Bar.x_y_z_1";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(6, atoms.Count());
            
            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Dot,
                AtomType.Identifier,
                AtomType.Dot,
                AtomType.Identifier,
                AtomType.Done
            }, atoms.Select(x => x.Type));

            Assert.Equal("foo", atoms.ToArray()[0].GetText(q));
            Assert.Equal(".", atoms.ToArray()[1].GetText(q));;
            Assert.Equal("Bar", atoms.ToArray()[2].GetText(q));;
            Assert.Equal("x_y_z_1", atoms.ToArray()[4].GetText(q));;
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
            
            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Slash,
                AtomType.Identifier,
                AtomType.Slash,
                AtomType.Identifier,
                AtomType.Done
            }, atoms.Select(x => x.Type));

            Assert.Equal("foo", atoms.ToArray()[0].GetText(q));
            Assert.Equal("/", atoms.ToArray()[1].GetText(q));;
            Assert.Equal("Bar", atoms.ToArray()[2].GetText(q));;
            Assert.Equal("x_y_z_1", atoms.ToArray()[4].GetText(q));
        }

        [Theory]
        [InlineData("foo/bar eq 123")]
        [InlineData("foo/bar eq 0")]
        public void ConditionEqString(string q)
        {
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(8, atoms.Count());

            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Slash,
                AtomType.Identifier,
                AtomType.Spaces,
                AtomType.Equal,
                AtomType.Spaces,
                AtomType.Integer,
                AtomType.Done
            }, atoms.Select(x => x.Type));
            
            Assert.Equal("foo", atoms.ToArray()[0].GetText(q));
        }

        [Fact]
        public void ConditionEqFloat()
        {
            var q = "foo/eqie eq 3.12";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(8, atoms.Count());

            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Slash,
                AtomType.Identifier,
                AtomType.Spaces,
                AtomType.Equal,
                AtomType.Spaces,
                AtomType.Float,
                AtomType.Done
            }, atoms.Select(x => x.Type));
        }

        [Fact]
        public void StringValue()
        {
            var q = "_poo_kie eq 'something'";
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(6, atoms.Count());

            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Identifier,
                AtomType.Spaces,
                AtomType.Equal,
                AtomType.Spaces,
                AtomType.String,
                AtomType.Done
            }, atoms.Select(x => x.Type));

            Assert.Equal("_poo_kie", atoms.ToArray()[0].GetText(q));
            Assert.Equal("'something'", atoms.ToArray()[4].GetText(q));
        }

        [Fact]
        public void Asterisk()
        {
            var q = "*";

            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(q));
            
            Assert.Equal(2, atoms.Count());

            Assert.Equal(new List<AtomType> 
            { 
                AtomType.Asterisk,
                AtomType.Done
            }, atoms.Select(x => x.Type));
        }
    }
}
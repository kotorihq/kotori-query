using System;
using KotoriQuery.AppException;
using Xunit;

namespace KotoriQuery.Tests
{
    public class DocumentDbSelect
    {
        [Theory]
        [InlineData("a,b desc")]
        [InlineData("x asc")]
        [InlineData("x eq 3")]
        public void InvalidTokens(string query)
        {
            var select = new Translator.DocumentDbSelect(query);
            Action a = () => select.GetTranslatedQuery();
            Assert.Throws<KotoriQueryException>(a);
        }

        [Fact]
        public void InvalidArgument()
        {
            Action a = () => new Translator.DocumentDbSelect(null);
            Assert.Throws<ArgumentNullException>(a);
        }

        [Theory]
        [InlineData("a,b", "c.a,c.b")]
        [InlineData("a,   b", "c.a, c.b")]
        [InlineData("foo/bar/something", "c.foo.bar.something")]
        [InlineData("foo//bar/something,something/bar/foo", "c.foo.bar.something,c.something.bar.foo")]
        [InlineData("foo//bar/something  ,  something/bar/foo", "c.foo.bar.something , c.something.bar.foo")]
        [InlineData("a,b/x,*,foo", "c.a,c.b.x,*,c.foo")]
        [InlineData("", "")]
        public void Selects(string query, string result)
        {
            var select = new Translator.DocumentDbSelect(query);
            var tran = select.GetTranslatedQuery();
            Assert.Equal(result, tran);
        }
    }
}
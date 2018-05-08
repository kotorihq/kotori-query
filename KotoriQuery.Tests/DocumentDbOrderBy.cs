using System;
using KotoriQuery.AppException;
using Xunit;

namespace KotoriQuery.Tests
{
    public class DocumentDbOrderBy
    {
        [Theory]
        [InlineData("a,b desc,*")]
        [InlineData("(damn)")]
        [InlineData("x eq 3")]
        public void InvalidTokens(string query)
        {
            var orderBy = new Translator.DocumentDbOrderBy(query);
            Action a = () => orderBy.GetTranslatedQuery();
            Assert.Throws<KotoriQueryException>(a);
        }

        [Fact]
        public void InvalidArgument()
        {
            Action a = () => new Translator.DocumentDbOrderBy(null);
            Assert.Throws<ArgumentNullException>(a);
        }

        [Theory]
        [InlineData("a,b", "c.a,c.b")]
        [InlineData("a,   b", "c.a, c.b")]
        [InlineData("first asc,second desc", "c.first ,c.second desc")]
        [InlineData("foo/bar/something", "c.foo.bar.something")]
        [InlineData("foo/bar/something,something/bar/foo", "c.foo.bar.something,c.something.bar.foo")]
        [InlineData("", "")]
        public void OrderBys(string query, string result)
        {
            var orderBy = new Translator.DocumentDbOrderBy(query);
            var tran = orderBy.GetTranslatedQuery();
            Assert.Equal(result, tran);
        }
    }
}
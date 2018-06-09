using System;
using System.Collections.Generic;
using KotoriQuery.AppException;
using KotoriQuery.Helpers;
using Xunit;

namespace KotoriQuery.Tests
{
    public class DocumentDbOrderBy
    {
        [Theory]
        [InlineData("a,b desc,*")]
        [InlineData("(damn)")]
        [InlineData("x eq 3")]
        [InlineData("x eq 'xoo'")]
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
        [InlineData("foo//bar/something,something/bar/foo", "c.foo.bar.something,c.something.bar.foo")]
        [InlineData("foo//bar/something desc ,  something/bar/foo", "c.foo.bar.something desc , c.something.bar.foo")]
        [InlineData("", "")]
        public void OrderBys(string query, string result)
        {
            var orderBy = new Translator.DocumentDbOrderBy(query);
            var tran = orderBy.GetTranslatedQuery();
            Assert.Equal(result, tran);
        }

        [Fact]
        public void OrderBysTransformation()
        {
            var query = "first/last asc,second desc,third";
            var orderBy = new Translator.DocumentDbOrderBy(query, new List<FieldTransformation>
            {
                new FieldTransformation("second", "Second2"),
                new FieldTransformation("first/last", "first2/last2"),
                new FieldTransformation("something", "anything"),
                new FieldTransformation("first2/last2", "first3/last3")
            });

            var tran = orderBy.GetTranslatedQuery();
            Assert.Equal("c.first2.last2 ,c.Second2 desc,c.third", tran);
        }
    }
}
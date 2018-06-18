using System;
using System.Collections.Generic;
using KotoriQuery.AppException;
using KotoriQuery.Helpers;
using Xunit;

namespace KotoriQuery.Tests
{
    public class DocumentDbFilter
    {
        [Theory]
        [InlineData("a,b desc")]
        [InlineData("x asc")]
        [InlineData("x desc")]
        [InlineData("megumi,*")]
        public void InvalidTokens(string query)
        {
            var filter = new Translator.DocumentDbFilter(query);
            Action a = () => filter.GetTranslatedQuery();
            Assert.Throws<KotoriQueryException>(a);
        }

        [Fact]
        public void InvalidArgument()
        {
            Action a = () => new Translator.DocumentDbFilter(null);
            Assert.Throws<ArgumentNullException>(a);
        }

        [Theory]
        [InlineData("x eq 'foo'", "c.x = 'foo'")]
        [InlineData("x ne 428", "c.x <> 428")]
        [InlineData("a/b gte 1 and a/b lte 10", "c.a.b >= 1 and c.a.b <= 10")]
        [InlineData("a/b ge 1 and a/b le 10", "c.a.b >= 1 and c.a.b <= 10")]
        [InlineData("oh gt 3.1 or (ah lt 4)", "c.oh > 3.1 or (c.ah < 4)")]
        [InlineData(@"moo eq 'koto\'ri'", @"c.moo = 'koto\'ri'")]
        [InlineData(@"moo eq 'koto\'ri\''", @"c.moo = 'koto\'ri\''")]
        [InlineData("", "")]
        public void Filter(string query, string result)
        {
            var filter = new Translator.DocumentDbFilter(query);
            var tran = filter.GetTranslatedQuery();
            Assert.Equal(result, tran);
        }

        [Fact]
        public void FilterTransformation()
        {
            var query = "a/b gte 1 and a/b lte 10 or foo/bar eq 'hi' or foo/bar eq 3";
            var filter = new Translator.DocumentDbFilter(query, new List<FieldTransformation>
                {
                    new FieldTransformation("a/b", "b/a"),
                    new FieldTransformation("b/a", "a/b", (v) => { return v.ToUpper(); }),
                    new FieldTransformation("foo/bar", "bar/foo", (v) => { return v.ToUpper() + "!"; })
                });

            var tran = filter.GetTranslatedQuery();
            Assert.Equal("c.b.a >= 1 and c.b.a <= 10 or c.bar.foo = 'HI!' or c.bar.foo = 3", tran);
        }
    }
}
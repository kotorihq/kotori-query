using System.Collections.Generic;

namespace KotoriQuery.Tokenize
{
    public enum AtomType 
    {
        Bad,
        Spaces,
        Comma,
        Identifier,
        Integer,
        Float,
        String,
        Asterisk,
        Slash,
        Plus,
        Minus,
        Equal,
        NotEqual,
        LessThan,
        LessThanThenEqual,
        GreaterThan,
        GreaterThanThenEqual,
        OpenParenthesis,
        CloseParenthesis,
        Ascending,
        Descending,
        And,
        Or,
        Done // used
    }

    public static class AtomTypeHelper 
    {
        private static Dictionary<string, AtomType> lookup = new Dictionary<string, AtomType> 
        { 
           { "Bad", AtomType.Bad },
           { "Spaces", AtomType.Spaces },
           { "Comma", AtomType.Comma },
           { "Identifier", AtomType.Identifier },
           { "Integer", AtomType.Integer },
           { "Float", AtomType.Float },
           { "String", AtomType.String },
           { "Asterisk", AtomType.Asterisk },
           { "Slash", AtomType.Slash },
           { "Plus", AtomType.Plus },
           { "Minus", AtomType.Minus },
           { "Equal", AtomType.Equal },
           { "NotEqual", AtomType.NotEqual },
           { "LessThan", AtomType.LessThan },
           { "LessThanThenEqual", AtomType.LessThanThenEqual },
           { "GreaterThan", AtomType.GreaterThan },
           { "GreaterThanThenEqual", AtomType.GreaterThanThenEqual },
           { "OpenParenthesis", AtomType.OpenParenthesis },
           { "CloseParenthesis", AtomType.CloseParenthesis },
           { "Ascending", AtomType.Ascending },
           { "Descending", AtomType.Ascending },
           { "And", AtomType.And },
           { "Or", AtomType.Or },
           { "Done", AtomType.Done }
        };

        public static AtomType ToEnum(string key) 
        {
            AtomType ret;

            return lookup.TryGetValue(key, out ret) ? ret : AtomType.Bad;
        }
    }
}
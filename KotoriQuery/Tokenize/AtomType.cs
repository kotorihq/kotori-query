using System.Collections.Generic;

namespace KotoriQuery.Tokenize
{
    public enum AtomType 
    {
        Bad, // used
        Spaces, // used
        Comma, // used
        Identifier, // used
        Integer, // used
        Float, // used
        String, // used
        Asterisk, // used
        Slash, // used
        Plus,
        Minus,
        Dot, // used
        Equal, // used
        NotEqual, // used
        LessThan, // used
        LessThanThenEqual, // used
        GreaterThan, // used
        GreaterThanThenEqual, // used
        OpenParenthesis,
        CloseParenthesis,
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
           { "Dot", AtomType.Dot },
           { "Equal", AtomType.Equal },
           { "NotEqual", AtomType.NotEqual },
           { "LessThan", AtomType.LessThan },
           { "LessThanThenEqual", AtomType.LessThanThenEqual },
           { "GreaterThan", AtomType.GreaterThan },
           { "GreaterThanThenEqual", AtomType.GreaterThanThenEqual },
           { "OpenParenthesis", AtomType.OpenParenthesis },
           { "CloseParenthesis", AtomType.CloseParenthesis },
           { "Done", AtomType.Done }
        };

        public static AtomType ToEnum(string key) 
        {
            AtomType ret;

            return lookup.TryGetValue(key, out ret) ? ret : AtomType.Bad;
        }
    }
}
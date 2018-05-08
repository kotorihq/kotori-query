using System.Collections.Generic;

namespace KotoriQuery.Tokenizer
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
}
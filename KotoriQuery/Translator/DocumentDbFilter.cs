using System.Collections.Generic;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class DocumentDbFilter : BaseDocumentDb, ITranslator
    {
        public IEnumerable<AtomType> AllowedAtomTypes => new List<AtomType>
        {
            AtomType.Identifier,
            AtomType.Spaces,
            AtomType.Comma,
            AtomType.Slash,
            AtomType.Integer,
            AtomType.Float,
            AtomType.String,
            AtomType.Equal,
            AtomType.NotEqual,
            AtomType.LessThan,
            AtomType.LessThanThenEqual,
            AtomType.GreaterThan,
            AtomType.GreaterThanThenEqual,
            AtomType.OpenParenthesis,
            AtomType.CloseParenthesis,
            AtomType.And,
            AtomType.Or,
            AtomType.Done
        };

        public DocumentDbFilter(string query) : base(query)
        {   
        }

        public string GetTranslatedQuery()
        {
            CheckAllowedAtoms(AllowedAtomTypes, _atoms);
            return Translate();
        }
    }
}
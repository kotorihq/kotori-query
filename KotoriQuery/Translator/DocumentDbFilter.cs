using System.Collections.Generic;
using KotoriQuery.Helpers;
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
            AtomType.Done,
        };

        public DocumentDbFilter(string query) : base(query, null)
        {   
        }

        public DocumentDbFilter(string query, IEnumerable<FieldTransformation> fieldTransformations) : base(query, fieldTransformations)
        {   
        }

        public string GetTranslatedQuery()
        {
            CheckAllowedAtoms(AllowedAtomTypes, _atoms);
            return Translate();
        }
    }
}
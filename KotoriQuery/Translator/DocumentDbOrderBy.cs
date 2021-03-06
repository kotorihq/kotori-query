using System.Collections.Generic;
using KotoriQuery.Helpers;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class DocumentDbOrderBy : BaseDocumentDb, ITranslator
    {
        public IEnumerable<AtomType> AllowedAtomTypes => new List<AtomType>
        {
            AtomType.Identifier,
            AtomType.Ascending,
            AtomType.Descending,
            AtomType.Spaces,
            AtomType.Comma,
            AtomType.Slash,
            AtomType.Done
        };

        public DocumentDbOrderBy(string query) : base(query, null)
        {   
        }

        public DocumentDbOrderBy(string query, IEnumerable<FieldTransformation> fieldTransformations) : base(query, fieldTransformations)
        {   
        }

        public string GetTranslatedQuery()
        {
            CheckAllowedAtoms(AllowedAtomTypes, _atoms);
            return Translate();
        }
    }
}
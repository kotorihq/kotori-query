using System.Collections.Generic;
using KotoriQuery.Helpers;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class DocumentDbSelect : BaseDocumentDb, ITranslator
    {
        public IEnumerable<AtomType> AllowedAtomTypes => new List<AtomType>
        {
            AtomType.Identifier,
            AtomType.Spaces,
            AtomType.Comma,
            AtomType.Slash,
            AtomType.Asterisk,
            AtomType.Done
        };

        public DocumentDbSelect(string query) : base(query, null)
        {   
        }

        public DocumentDbSelect(string query, IEnumerable<FieldTransformation> fieldTransformations) : base(query, fieldTransformations)
        {   
        }

        public string GetTranslatedQuery()
        {
            CheckAllowedAtoms(AllowedAtomTypes, _atoms);
            return Translate();
        }
    }
}
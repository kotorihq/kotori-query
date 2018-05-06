using System.Collections.Generic;
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
            AtomType.Comma
        };

        public DocumentDbOrderBy(string query) : base(query)
        {
            var atoms = GetAtoms(query);
            
        }

        public string GetTranslatedQuery()
        {
            throw new System.NotImplementedException("TODO");
            //CheckAllowedAtoms(AllowedAtomTypes, )
        }
    }
}
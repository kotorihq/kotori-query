using System.Collections.Generic;
using System.Text;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class BaseDocumentDb : BaseTranslator
    {
        protected const string Prefix = "c";
        protected string _query { get; private set; }
        protected IEnumerable<Atom> _atoms { get; private set; }

        public BaseDocumentDb(string query)
        {
            _atoms = GetAtoms(query);
            _query = query;
        }

        protected string Translate()
        {
            var result = new StringBuilder();
            bool identifierChain = false;

            foreach(var a in _atoms)
            {
                switch(a.Type)
                {
                    case AtomType.Ascending:
                        identifierChain = false;
                        break;

                    case AtomType.Descending:
                        identifierChain = false;
                        result.Append("desc");
                        break;

                    case AtomType.Comma:
                        identifierChain = false;
                        result.Append(",");
                        break;

                    case AtomType.Slash:
                        if (!identifierChain) 
                            result.Append(".");
                        else
                            result.Append(".");
                        break;

                    case AtomType.Spaces:
                        identifierChain = false;
                        result.Append(" ");
                        break;

                    case AtomType.Identifier:
                        if (!identifierChain)
                            result.Append(Prefix + ".");

                        identifierChain = true;

                        result.Append(GetAtomText(a, _query));
                        break;

                    case AtomType.Done:
                        break;
                }
            }

            return result.ToString();
        }
    }
}
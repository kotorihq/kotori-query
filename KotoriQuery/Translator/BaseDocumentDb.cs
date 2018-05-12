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
            Atom previous = null;

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
                        {
                            result.Append(".");
                        }
                        else
                        {
                            if (previous?.Type != AtomType.Slash)
                                result.Append(".");
                        }
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

                    case AtomType.Asterisk:
                        result.Append("*");
                        break;
                    
                    case AtomType.Integer:
                    case AtomType.Float:
                        result.Append(GetAtomText(a, _query));
                        break;

                    case AtomType.String:
                        var original = GetAtomText(a, _query);
                        var clean = GetCleanQuotedString(original);
                        var json = GetJsonString(clean);
                    
                        result.Append("'" + json + "'");
                        break;

                    case AtomType.Equal:
                        result.Append("=");
                        break;

                    case AtomType.NotEqual:
                        result.Append("<>");
                        break;

                    case AtomType.LessThan:
                        result.Append("<");
                        break;

                    case AtomType.GreaterThan:
                        result.Append(">");
                        break;

                    case AtomType.LessThanThenEqual:
                        result.Append("<=");
                        break;

                    case AtomType.GreaterThanThenEqual:
                        result.Append(">=");
                        break;

                    case AtomType.And:
                        result.Append("and");
                        break;

                    case AtomType.Or:
                        result.Append("or");
                        break;

                    case AtomType.OpenParenthesis:
                        result.Append("(");
                        break;

                    case AtomType.CloseParenthesis:
                        result.Append(")");
                        break;

                    case AtomType.Done:
                        break;
                }

                previous = a;
            }

            return result.ToString();
        }
    }
}
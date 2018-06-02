using System.Collections.Generic;
using System.Linq;
using System.Text;
using KotoriQuery.Helpers;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class BaseDocumentDb : BaseTranslator
    {
        protected const string Prefix = "c";
        protected string _query { get; private set; }
        protected IEnumerable<Atom> _atoms { get; private set; }
        protected IEnumerable<FieldTransformation> _fieldTransformations { get; private set;}

        public BaseDocumentDb(string query, IEnumerable<FieldTransformation> fieldTransformations)
        {
            _atoms = GetAtoms(query);
            _fieldTransformations = fieldTransformations;
            _query = query;
        }

        protected string Translate()
        {
            var result = new StringBuilder();
            var identifierChain = new List<string>();
            Atom previous = null;

            foreach(var a in _atoms)
            {
                if (a.Type != AtomType.Identifier &&
                    a.Type != AtomType.Slash)
                {
                    ProcessIdentifier(ref identifierChain, ref result);
                }

                switch(a.Type)
                {
                    case AtomType.Ascending:
                        break;

                    case AtomType.Descending:
                        result.Append("desc");
                        break;

                    case AtomType.Comma:
                        result.Append(",");
                        break;

                    case AtomType.Slash:
                        if (!identifierChain.Any()) 
                        {
                            identifierChain.Add(".");
                        }
                        else
                        {
                            if (previous?.Type != AtomType.Slash)
                                identifierChain.Add(".");
                        }
                        break;

                    case AtomType.Spaces:
                        result.Append(" ");
                        break;

                    case AtomType.Identifier:
                        identifierChain.Add(GetAtomText(a, _query));
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

        void ProcessIdentifier(ref List<string> chain, ref StringBuilder sb)
        {
            if (!chain.Any())
                return;

            sb.Append(Prefix);
            sb.Append(".");

            foreach(var c in chain)
            {    
                sb.Append(c);                
            }

            chain = new List<string>();
        }
    }
}
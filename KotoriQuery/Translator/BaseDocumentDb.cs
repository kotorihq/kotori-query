using System.Collections.Generic;
using System.Linq;
using System.Text;
using KotoriQuery.Helpers;
using KotoriQuery.Tokenizer;
using Sushi2;

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
            string lastIdentifier = null;
            Atom previous = null;

            foreach(var a in _atoms)
            {
                if (a.Type != AtomType.Identifier &&
                    a.Type != AtomType.Slash)
                {
                    if (identifierChain.Any())
                    {
                        var r = ProcessIdentifier(ref identifierChain, ref lastIdentifier);
                        result.Append(r);
                    }
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
                        else if (previous?.Type != AtomType.Slash)
                        {
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
                        lastIdentifier = null;
                        break;

                    case AtomType.String:
                        var original = GetAtomText(a, _query);
                        var clean = GetCleanQuotedString(original);

                        if (_fieldTransformations != null &&
                            _fieldTransformations.Any() &&
                            lastIdentifier != null)
                        {
                            var t = _fieldTransformations.FirstOrDefault(x => x.From == lastIdentifier);

                            if (t != null &&
                                t.Translator != null)
                            {
                                clean = t.Translator(clean);
                                lastIdentifier = null;
                            }
                        }
                        
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

        string ProcessIdentifier(ref List<string> chain, ref string lastIdentifier)
        {
            lastIdentifier = null;

            if (!chain.Any())
                return string.Empty;  

            var sb = new StringBuilder();
            sb.Append(Prefix);
            sb.Append(".");

            var newChain = GetTransformedIdentifierChain(_fieldTransformations, chain, ref lastIdentifier);

            foreach(var c in newChain)
            {    
                sb.Append(c);                
            }

            chain = new List<string>();
            return sb.ToString();
        }

        IEnumerable<string> GetTransformedIdentifierChain(IEnumerable<FieldTransformation> transformations, List<string> chain, ref string lastIdentifier)
        {
            if (transformations == null ||
                !transformations.Any())
                return chain;

            var m = chain.ToImplodedString("");
            var t = transformations.FirstOrDefault(x => x.From.Replace("/", ".") == chain.ToImplodedString(""));

            if (t == null)
                return chain;

            lastIdentifier = t.From;
            
            if (t.To == null)
                return chain;
            
            var result = t.To?.Split(new [] { '/' });
            var result2 = new List<string>();

            foreach(var r in result)
            {
                if (result2.Any())
                    result2.Add(".");

                result2.Add(r);
            }

            return result2;
        } 
    }
}
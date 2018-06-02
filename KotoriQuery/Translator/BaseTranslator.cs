using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KotoriQuery.AppException;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public class BaseTranslator
    {
        protected IEnumerable<Atom> GetAtoms(string query)
        {
            if (query == null)
                throw new System.ArgumentNullException(nameof(query));
            
            var atoms = new Atomizer<StringCharacterReader>(new StringCharacterReader(query));

            return atoms;
        }

        protected string GetAtomText(Atom atom, string query)
        {
            return atom.GetText(query);    
        }

        protected void CheckAllowedAtoms(IEnumerable<AtomType> allowedAtomTypes, IEnumerable<Atom> realAtoms)
        {
            if (allowedAtomTypes == null)
                throw new System.ArgumentNullException(nameof(allowedAtomTypes));

            if (realAtoms == null)
                throw new System.ArgumentNullException(nameof(realAtoms));

            var atom = realAtoms.FirstOrDefault(a => allowedAtomTypes.All(a2 => a2 != a.Type));

            if (atom != null)
                throw new KotoriQueryException($"Atom type {atom.Type} is not allowed.");

            return;
        }

        protected string GetCleanQuotedString(string s)
        {
            if (s == null || 
                s.Length <= 2) 
            {
                return string.Empty;
            }

            return s.Substring(1).Substring(0, s.Length - 2);
        }

        protected string GetJsonString(string s)
        {
            if (s == null || 
                s.Length == 0) 
            {
                return string.Empty;
            }

            var c = '\0';
            int i;
            var len = s.Length;
            var sb = new StringBuilder(len + 4);
            String  t;

            for (i = 0; i < len; i++) 
            {
                c = s[i];

                switch (c) 
                {
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    default:
                        if (c < ' ') 
                        {
                            t = "000" + String.Format("X", c);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        } 
                        else 
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
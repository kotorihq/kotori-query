using System.Collections.Generic;
using System.Linq;
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

        protected void CheckAllowedAtoms(IEnumerable<Atom> allowedAtoms, IEnumerable<Atom> realAtoms)
        {
            if (allowedAtoms == null)
                throw new System.ArgumentNullException(nameof(allowedAtoms));

            if (realAtoms == null)
                throw new System.ArgumentNullException(nameof(realAtoms));

            var atom = realAtoms.FirstOrDefault(a => allowedAtoms.All(a2 => a2.Type != a.Type));

            if (atom != null)
                throw new KotoriQueryException($"Atom type {atom.Type} is not allowed.");

            return;
        }
    }
}
using System.Collections.Generic;
using KotoriQuery.Tokenizer;

namespace KotoriQuery.Translator
{
    public interface ITranslator
    {
        IEnumerable<AtomType> AllowedAtomTypes { get; }
        string GetTranslatedQuery();
    }
}
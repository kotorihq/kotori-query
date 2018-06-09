using System;

namespace KotoriQuery.Helpers
{
    public class FieldTransformation
    {
        public string From { get; }
        public string To { get; }
        public Func<string, string> Translator { get; }

        public FieldTransformation(string from, string to, Func<string, string> translator = null)
        {
            From = from ?? throw new System.ArgumentNullException(nameof(from));
            To = to;
            Translator = translator;
        }
    }
}
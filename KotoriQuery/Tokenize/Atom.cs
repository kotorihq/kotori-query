using System;

namespace KotoriQuery.Tokenize 
{
    public struct Atom 
    {
        public static readonly Atom Done = new Atom(AtomType.Done, TextPosition.Done, TextPosition.Done);

        public readonly AtomType Type;

        public readonly TextPosition Start;

        public readonly TextPosition End;

        public Atom(AtomType type, TextPosition start, TextPosition end) 
        {
            if (start.Offset > end.Offset)
                new ArgumentOutOfRangeException(nameof(start), "Index out of range.");

            Type = type;
            Start = start;
            End = end;
        }

        public string GetText(string text) 
        {
            if (Type.Equals(AtomType.Done)) {
                return String.Empty;
            }

            if (Start.Offset < text.Length && End.Offset < text.Length) {
                return text.Substring(Start.Offset, End.Offset - Start.Offset + 1);
            }

            return String.Empty; 
        }
    }
}
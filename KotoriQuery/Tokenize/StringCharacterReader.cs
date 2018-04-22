using System;

namespace KotoriQuery.Tokenize
{
    /// <summary>
    /// Converts each character (maximum UTF-16 i.e. 4 bytes) to an Int32 wrapped as Char32 allowing
    /// for BPM surrogates.  The surrogate pairing sets the size of a character to Int32.
    /// </summary>
    public struct StringCharacterReader : ICharacterReader 
    {
        private readonly string _text;

        public StringCharacterReader(string text) 
        {
            _text = text;
        }

        public int Start => 0;

        /// <summary>
        /// Get value at position which bumps the position twice if 
        /// a high surrogate otherwise just once
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Char32? TryGet(ref int position) 
        {
            if (position < _text.Length) 
            {
                Char c = _text[position];
                position++;

                return char.IsHighSurrogate(c) ? WithSurrogate(ref position, c) : c;
            }

            position = _text.Length;

            return null;
        }

        /// <summary>
        /// Joins the first UTF-16 with the second into a Int32
        /// </summary>
        /// <param name="position"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private int WithSurrogate(ref int position, char c) 
        {
            if (position < _text.Length) 
            {
                Char after = _text[position];
                position++;

                if (char.IsLowSurrogate(after)) 
                    return char.ConvertToUtf32(c, after);

                throw new InvalidOperationException("Unexpected character after high-surrogate char.");
            }
            
            throw new ArgumentOutOfRangeException("Unexpected END after high-surrogate char.");
        }
    }
}

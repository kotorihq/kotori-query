using System;
using System.Collections;
using System.Collections.Generic;

namespace KotoriQuery.Tokenize
{
    public class Atomizer<TReader> : IEnumerable<Atom> where TReader : struct, ICharacterReader 
    {
        private Atom _atom;
        private Char32 _c;
        private readonly TReader _reader;
        private TextPosition _position;
        private TextPosition _nextPosition;
        private const int End = -1;

        public Atomizer(TReader reader) 
        {
            _reader = reader;
        }

        public Enumerator GetEnumerator() 
        {
            return new Enumerator(this);
        }

        IEnumerator<Atom> IEnumerable<Atom>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
             return GetEnumerator();
        }

        private void Reset() 
        {
            _position = new TextPosition(0);
            _nextPosition = new TextPosition(0);
            _c = ReadNextCharacter();

            _atom = new Atom();
        }

        /// <summary>
        /// Is atom equal to Done otherwise next atom
        /// </summary>
        /// <returns></returns>
        private bool MoveNext() 
        {
            if (_atom.Type == AtomType.Done) 
            {
                return false;
            }

            NextAtom();

            return true;
        }

        /// <summary>
        /// Reader gets value at current position then bumps the next position
        /// </summary>
        /// <returns></returns>
        private Char32 ReadNextCharacter() 
        {
            try 
            {
                int curr = _position.Offset;
                var nullable = _reader.TryGet(ref curr);

                _nextPosition.Offset = curr;

                if (nullable.HasValue) {
                    return nullable.Value;
                }

                return End; // dead-end (-1)
            } 
            catch (Exception) {
                // swallow
            }

            return 0;  // nothing
        }

        /// <summary>
        /// Next character (next position to current then engage the reader)
        /// </summary>
        private void NextCharacter() 
        {
            _position = _nextPosition;
            _c = ReadNextCharacter();
        }

        /// <summary>
        /// Next atom
        /// </summary>
        private void NextAtom() 
        {
            var beginning = _position;

            switch (_c)
            {
                case '*':
                    NextCharacter();
                    _atom = new Atom(AtomType.Asterisk, beginning, beginning);
                    break;
                case '/':
                    NextCharacter();
                    _atom = new Atom(AtomType.Slash, beginning, beginning);
                    break;
                case '+':
                    NextCharacter();
                    _atom = new Atom(AtomType.Plus, beginning, beginning);
                    break;
                case '-':
                    NextCharacter();
                    _atom = new Atom(AtomType.Minus, beginning, beginning);
                    break;
                case '.':
                    NextCharacter();
                    _atom = new Atom(AtomType.Dot, beginning, beginning);
                    break;
                case '!':
                    NextCharacter();

                    if (_c == '=') {
                        NextCharacter();
                        _atom = new Atom(AtomType.ExclamationThenEqual, beginning, _position);
                        break;
                    }

                    _atom = new Atom(AtomType.Exclamation, beginning, beginning);
                    break;
                case '=':
                    NextCharacter();

                    if (_c == '=') 
                    {
                        NextCharacter();
                        _atom = new Atom(AtomType.EqualPair, beginning, _position);
                        break;
                    }

                    _atom = new Atom(AtomType.Equal, beginning, beginning);
                    break;
                case '<':
                    NextCharacter();

                    if (_c == '=') {
                        NextCharacter();
                        _atom = new Atom(AtomType.LessThanThenEqual, beginning, _position);
                        break;
                    }

                    _atom = new Atom(AtomType.LessThan, beginning, beginning);
                    break;
                case '>':
                    NextCharacter();

                    if (_c == '=') {
                        NextCharacter();
                        _atom = new Atom(AtomType.GreaterThanThenEqual, beginning, _position);
                        break;
                    }

                    _atom = new Atom(AtomType.GreaterThan, beginning, beginning);
                    break;
                case '(':
                    NextCharacter();
                    _atom = new Atom(AtomType.OpenParenthesis, beginning, beginning);
                    break;
                case ')':
                    NextCharacter();
                    _atom = new Atom(AtomType.CloseParenthesis, beginning, beginning);
                    break;
                case ',':
                    NextCharacter();
                    _atom = new Atom(AtomType.Comma, beginning, beginning);
                    break;
                case '"':
                    ConsumeString(beginning);
                    break;
                case '&':
                    NextCharacter();

                    if (_c == '&') {
                        NextCharacter();
                        _atom = new Atom(AtomType.AmpersandPair, beginning, _position);
                        break;
                    }
                    
                    _atom = new Atom(AtomType.Ampersand, beginning, beginning);
                    break;
                case '|':
                    NextCharacter();

                    if (_c == '|') {
                        NextCharacter();
                        _atom = new Atom(AtomType.PipePair, beginning, _position);
                        break;
                    }

                    _atom = new Atom(AtomType.Pipe, beginning, beginning);
                    break;
                case End:
                    _atom = Atom.Done;
                    break;
                default:
                    // stop if read whitespaces
                    if (TryConsumeWhitespaces()) {
                        break;
                    }

                    // stop if read identifiers
                    if (TryConsumeIdentifier()) {
                        break;
                    }

                    // stop if read numbers
                    if (TryConsumeNumber()) {
                        break;
                    }

                    // finally if nothing read then bad
                    _atom = new Atom(AtomType.Bad, _position, _position);
                    NextCharacter();
                    break;
            }
        }

        /// <summary>
        /// Store next characters while white spaces
        /// </summary>
        /// <returns></returns>
        private bool TryConsumeWhitespaces() {
            var beginning = _position;
            var finishing = _position;

            while (Tester.IsWhiteSpace(_c)) {
                finishing = _position;
                NextCharacter();
            }

            if (!beginning.Equals(_position)) {
                _atom = new Atom(AtomType.Spaces, beginning, finishing);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Store next characters while identifiers or an understore (special "this" character)
        /// </summary>
        /// <returns></returns>
        private bool TryConsumeIdentifier() {
            var beginning = _position;
            var finishing = _position;

            var startWithUnderscore = '_' == _c;

            var isHead = true;
            while (isHead ? Tester.IsIdentifierHead(_c) : Tester.IsIdentifierTailing(_c)) {
                finishing = _position;
                NextCharacter();

                isHead = false;
            }

            if (!beginning.Equals(_position)) {
                var length = finishing.Offset - beginning.Offset;

                if (startWithUnderscore && length == 0) {
                    _atom = new Atom(AtomType.Understore, beginning, finishing);
                } else {
                    _atom = new Atom(AtomType.Identifier, beginning, finishing);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Store next characters while numbers (long/integer or double/float)
        /// </summary>
        /// <returns></returns>
        private bool TryConsumeNumber() {
            var beginning = _position;
            var finishing = _position;
            var isFloat = false;

            while (Tester.IsDigit(_c)) {
                finishing = _position;
                NextCharacter();
            }

            if (_c == '.') {
                NextCharacter();

                if (!Tester.IsDigit(_c)) {
                    _atom = new Atom(AtomType.Bad, beginning, finishing);
                    return true; // read then stored
                }

                isFloat = true;

                while (Tester.IsDigit(_c)) {
                    finishing = _position;
                    NextCharacter();
                }
            }

            if (!beginning.Equals(_position)) {
                _atom = new Atom(isFloat ? AtomType.Float : AtomType.Integer, beginning, finishing);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Consume strings
        /// </summary>
        /// <param name="beginning"></param>
        private void ConsumeString(TextPosition beginning) {
            var finishing = _position;
            Char32 startChar = _c;

            NextCharacter();  // skip quote
            while (true) {
                if (TryConsumeChar(ref finishing, startChar)) {
                    if (_c == startChar) { // ending quote or loop until the reader reaches a hard limit
                        finishing = _position;
                        NextCharacter(); // skip quote

                        break;
                    }
                } else {
                    _atom = new Atom(AtomType.Bad, beginning, finishing);
                    return;
                }
            }

            _atom = new Atom(AtomType.String, beginning, finishing);
        }

        /// <summary>
        /// Store next characts while is Char (i.e. not at end or match on start character)
        /// </summary>
        /// <param name="finishing"></param>
        /// <param name="startChar"></param>
        /// <returns></returns>
        private bool TryConsumeChar(ref TextPosition finishing, Char32 startChar) {
            if (_c == End) {
                return false;
            }

            if (_c != startChar) {
                finishing = _position;
                NextCharacter();
            }

            return true;
        }

        /// <summary>
        /// Peek if characters are coming
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        private bool IsNext(string characters) {
            var savedPosition = _position;
            var savedNextPosition = _nextPosition;

            var match = true;
            foreach (char c in characters) {
                if (!ReadNextCharacter().Equals(c)) {
                    match = false;
                    break;
                }
            }

            _position = savedPosition;
            _nextPosition = savedNextPosition;

            return match;
        }

        /// <summary>
        /// Exposes private methods to Atomizer as an enumerator of atoms
        /// </summary>
        public struct Enumerator: IEnumerator<Atom> {
            private readonly Atomizer<TReader> _atomizer;

            public Enumerator(Atomizer<TReader> atomizer) {
                _atomizer = atomizer;
                atomizer.Reset();
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection
            /// </summary>
            /// <returns></returns>
            public bool MoveNext() {
                return _atomizer.MoveNext();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset() {
                _atomizer.Reset();
            }

            public Atom Current => _atomizer._atom;

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current => Current;

            /// <summary>
            /// Struct (bye-bye)
            /// </summary>
            public void Dispose() {

            }
        }
    }
}
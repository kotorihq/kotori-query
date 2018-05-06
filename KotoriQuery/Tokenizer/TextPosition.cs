namespace KotoriQuery.Tokenizer
{
    public struct TextPosition
    {
        public static readonly TextPosition Done = new TextPosition(-1);

        public int Offset { get; set; }

        public TextPosition(int offset) 
        {
            Offset = offset;
        }

        public TextPosition Next(int offset = 1) 
        {
            return new TextPosition(Offset + offset);
        }
    }
}
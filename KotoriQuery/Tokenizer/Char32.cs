namespace KotoriQuery.Tokenizer
{
    public struct Char32 
    {
        public int Code { get; }

        public Char32(int code) 
        {
            Code = code;
        }

        public static implicit operator int(Char32 c)
        {
            return c.Code;
        }

        public static implicit operator Char32(int c)
        {
            return new Char32(c);
        }

        public override string ToString() 
        {
            if (Code == -1)
                return string.Empty;

            var c = char.ConvertFromUtf32(Code);
            
            return c;
        }
    }
}
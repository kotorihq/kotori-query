namespace KotoriQuery.Helpers
{
    public class FieldTransformation
    {
        public string From { get; set; }
        public string To { get; set; }

        public FieldTransformation(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}
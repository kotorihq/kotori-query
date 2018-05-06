namespace KotoriQuery.Translator
{
    public class BaseDocumentDb : BaseTranslator
    {
        protected const string Prefix = "c";
        protected string _query;

        public BaseDocumentDb(string query)
        {
            _query = query;
        }
    }
}
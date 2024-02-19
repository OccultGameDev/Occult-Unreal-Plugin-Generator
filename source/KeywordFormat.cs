namespace UnrealPluginGenerator
{
    /// <summary>
    /// Keyword Formats a value dynamically, it is a helper to simplify the creation of Formatters
    /// </summary>
    internal struct KeywordFormat
    {
        public KeywordFormat()
        {
            Format = "{UNDEFINED_KEY}";
            ValueGetter = null;
        }

        public KeywordFormat(string inFormat, ValueDelegate valueDelegate)
        {
            Format = inFormat;
            ValueGetter = valueDelegate;
        }

        // This is the value sought in the Template
        public string Format { get; set; } = "{UNDEFINED_KEY}";

        public delegate string ValueDelegate();
        public ValueDelegate? ValueGetter { get; set; }

        public string ReplaceKeywords(string inTextToFix)
        {
            if(null == ValueGetter)
            {
                return inTextToFix;
            }
            return inTextToFix.Replace(Format, ValueGetter());
        }
    }
}

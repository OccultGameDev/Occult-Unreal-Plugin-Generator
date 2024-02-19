using System.Text;

namespace UnrealPluginGenerator
{
    /// <summary>
    /// Streams Text to a TextBox
    /// </summary>
    internal class ErrorConsoleStreamWriter : TextWriter
    {
        TextBox? OutputText = null;
        public ErrorConsoleStreamWriter(TextBox output)
        {
            OutputText = output;
            if(null != OutputText)
            {
                OutputText.ScrollBars = ScrollBars.Vertical;
            }
        }

        public override void Write(char value)
        {
            base.Write(value);
            if(null != OutputText)
            {
                
                OutputText.AppendText(value.ToString());
                OutputText.SelectionStart = OutputText.Text.Length-1;
                OutputText.ScrollToCaret();
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}

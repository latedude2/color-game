using System.Text;


/////
// from https://stackoverflow.com/questions/272633/add-spaces-before-capital-letters
/////


namespace CustomExtensions
{
    //Extension methods must be defined in a static class
    public static class StringExtension {
        public static string AddSpacesToSentence(this string text) {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++) {
                if (text[i - 1] != ' '
                        && (char.IsUpper(text[i]) || char.IsNumber(text[i])))
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
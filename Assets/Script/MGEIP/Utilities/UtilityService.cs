using System.Text.RegularExpressions;

public static class UtilityService
{
    public static string RemoveRichTextTags(string input)
    {
        input = input.Replace("<br>", " ");

        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}

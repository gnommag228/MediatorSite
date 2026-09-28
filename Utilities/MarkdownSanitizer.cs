namespace MediatorSite.Utilities;

 public static class MarkdownSanitizer
 {
    public static string Sanitize(string? text)
    {
      return (text ?? "")
         .Replace("*", "\\*")
         .Replace("_", "\\_")
         .Replace("`", "\\`");
    } 
 }
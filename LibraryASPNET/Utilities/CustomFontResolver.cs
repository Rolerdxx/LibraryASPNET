using PdfSharp.Fonts;
using System.IO;

public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        if (faceName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
        {
            
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "verdana.ttf");
            return File.ReadAllBytes(fontPath);
        }

        return null;
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
        {
           
            return new FontResolverInfo("Verdana");
        }

        return null;
    }
}

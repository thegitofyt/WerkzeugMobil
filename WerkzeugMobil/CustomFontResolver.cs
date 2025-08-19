
using PdfSharpCore.Fonts;
using System;
using System.IO;
using System.Reflection;

namespace WerkzeugMobil
{
    public class CustomFontResolver : IFontResolver
    {

        public string DefaultFontName => "verdana";
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("verdana", StringComparison.OrdinalIgnoreCase))
            {
                if (isBold && isItalic)
                    return new FontResolverInfo("verdana-bold-italic");
                if (isBold)
                    return new FontResolverInfo("verdana-bold");
                return new FontResolverInfo("verdana");
            }
            return new FontResolverInfo("Fallback-Regular");
        }

        public byte[] GetFont(string faceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string resourceName = faceName switch
            {
                "verdana" => "WerkzeugMobil.Fonts.verdana.ttf",
                "verdana-bold" => "WerkzeugMobil.Fonts.verdana-bold.ttf",
                "verdana-bold-italic" => "WerkzeugMobil.Fonts.verdana-bold-italic.ttf",
                _ => throw new FileNotFoundException($"Font {faceName} not found.")
            };

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException($"Embedded font resource '{resourceName}' nicht gefunden.");

            byte[] fontData = new byte[stream.Length];
            stream.Read(fontData, 0, fontData.Length);
            return fontData;
        }
    }
}
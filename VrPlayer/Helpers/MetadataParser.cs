using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SE.Halligang.CsXmpToolkit;
using SE.Halligang.CsXmpToolkit.Schemas;

namespace VrPlayer.Helpers
{
    public class MetadataParser
    {
        public MetadataParser(string filePath)
        {
            // The above code uses the XMP Toolkit. The corresponding code using
            // the C# XMP Toolkit would be:
            using (Xmp xmp = Xmp.FromFile(filePath, XmpFileMode.ReadOnly))
            {
                DublinCore dc = new DublinCore(xmp);
                if (dc.Source != null)
                {
                    // Use source here...
                }
                foreach (string subject in dc.Subject)
                {
                    // Use subject here...
                }
                foreach (LangEntry langEntry in dc.Title)
                {
                    // Use title, langEntry.Language and langEntry.Value, here...
                }
            }
        }

    }
}

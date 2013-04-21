using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SE.Halligang.CsXmpToolkit;
using SE.Halligang.CsXmpToolkit.Schemas;

namespace VrPlayer.Models.Metadata
{
    //Todo: Use generic to parse other type than MetadataResult
    public class MetadataParser
    {
        public const string Namespace = "http://ns.vrplayer.tv/";

        private string _filePath;

        public MetadataParser(string filePath)
        {
            _filePath = filePath;
        }

        public MetadataResult Parse()
        {
            var result = new MetadataResult();
            using (var xmp = Xmp.FromFile(_filePath, XmpFileMode.ReadOnly))
            {
                var properties = typeof(MetadataResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    try
                    {
                        string propValue;
                        PropertyFlags flags;
                        xmp.XmpCore.GetProperty(Namespace, p.Name, out propValue, out flags);
                        if (!string.IsNullOrEmpty(propValue))
                        {
                            p.SetValue(result, propValue, null);
                        }     
                    }
                    catch(Exception exc)
                    {
                        //Todo: log
                    }
                }
            }
            return result;
        }
    }
}

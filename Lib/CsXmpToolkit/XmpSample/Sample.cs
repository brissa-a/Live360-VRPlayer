// *****************************************************************************
// Sample.cs
// A sample class to demonstrate the usage of C# XMP Toolkit
//
// In order to run this code you must:
// - Add a reference to CsXmpToolkit.dll
//   (already done for the XmpSample project)
// - Copy the XmpToolkit.dll to the output directory
//   (this is done automatically for the XmpSample project,
//    provided you've kept the folder hierarchy)
// *****************************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SE.Halligang.CsXmpToolkit;
using SE.Halligang.CsXmpToolkit.Schemas;

namespace XmpSample
{
	public static class Sample
	{
		public static void Run()
		{
			// The C# XMP Toolkit consists of two parts,
			// - a wrapper around the XMP Toolkit
			// - schema helper classes

			// Create an instance of CsXmpToolkit logfile (useful for tracking down bugs).
			// To disable logging, remove the following line:
			LogFile log = new LogFile("CsXmpToolkit", Application.StartupPath + @"\program.log.txt", true, 4096000);
			// If you wish to use the logfile instance in your application,
			// you can retrieve it by using the following code:
			// LogFile log = LogFile.GetInstance("CsXmpToolkit");
			// NOTE! In order to get logging from the C# Xmp Toolkit the key must be "CsXmpToolkit"!
			// NOTE! In order to get an instance, the logfile has to be created first!
			// NOTE! In order for logging to be enabled, your project must have a file named app.config!

			// Change the following to the full path of a jpeg image of your choice.
			filePath = @"c:\myfolder\myimage.jpg";
			// After updating the filePath, remove the following line:
			throw new ApplicationException("Please edit the Run() method of Sample.cs before starting the application.");

			// Create a thumbnail image and store it Base64 encoded.
			// Do this before performing any XMP procedures on the file since
			// the XMP Toolkit requires exclusive access to the file.
			thumbnail = ThumbnailFromFile(filePath);

			// Test the XMP Toolkit
			TestToolkit();

			// Test the schema helper classes of C# XMP Toolkit
			TestSchemas();
		}

		private static string filePath;
		private static Thumbnail thumbnail;

		#region Toolkit

		/// <summary>
		/// Please download the XMP Toolkit from Adobe
		/// and read the documentation for more information.
		/// http://www.adobe.com/devnet/xmp/
		/// </summary>
		private static void TestToolkit()
		{
			if (XmpCore.Initialize() && XmpFiles.Initialize())
			{
				using (XmpFiles xmpFiles = new XmpFiles(filePath, FileFormat.Unknown, OpenFlags.OpenForRead | OpenFlags.OpenUseSmartHandler | OpenFlags.OpenCacheTNail))
				{
					using (XmpCore xmpCore = new XmpCore())
					{
						if (xmpFiles.GetXmp(xmpCore))
						{
							// Read the Dublin Core Schema
							// NOTE: Only source, subject and title are read!
							string dcNS = "http://purl.org/dc/elements/1.1/";
							PropertyFlags options;

							string source;
							xmpCore.GetProperty(dcNS, "source", out source, out options);
							if (source != null)
							{
								// Use source here...
							}

							XmpIterator iter = new XmpIterator(xmpCore, dcNS, "subject", IteratorMode.JustChildren);
							string schemaNS;
							string propPath;
							string propValue;
							while (iter.Next(out schemaNS, out propPath, out propValue, out options))
							{
								// Use subject (propValue) here...
							}

							iter = new XmpIterator(xmpCore, dcNS, "title", IteratorMode.JustChildren);
							while (iter.Next(out schemaNS, out propPath, out propValue, out options))
							{
								string langNS = "http://www.w3.org/XML/1998/namespace";
								string language;
								if (xmpCore.GetQualifier(dcNS, propPath, langNS, "lang", out language, out options))
								{
									// Use title, language and value (propValue), here...
								}
							}
						}
					}
					xmpFiles.CloseFile(CloseFlags.None);
				}

				XmpFiles.Terminate();
				XmpCore.Terminate();
			}

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

		#endregion

		#region Schemas

		private static void TestSchemas()
		{
			// Get handler capabilities. This will show what file formats are supported
			// and what each handler is capable of. Most important is whether the handler
			// allows you to insert new XMP or only edit existing XMP.
			// Set a breakpoint on the following line and inspect the value.
			string handlerCapabilities = HandlerCapabilities();

			// Create an Xmp object with the "using" keyword for
			// an automatic Dispose.
			// NOTE! Nothing will be written to file before Dispose
			//       is called!

			// This will automatically Initialize the Toolkit when
			// created and Terminate the toolkit when disposed.
			// If more than one file is to be opened then it is
			// more efficient to manually initialize and terminate
			// the toolkit. Uncomment the following line to do so:
			//Xmp.Initialize();

			// ReadWrite will read XMP and allows update.
			// ReadOnly will read XMP and won't update.
			// WriteOnly will not read XMP, even if it exists, and will overwrite when updating.
			using (Xmp xmp = Xmp.FromFile(filePath, XmpFileMode.ReadWrite))
			{
				// See the original XMP.
				// Set a breakpoint on the following line and
				// inspect the xmpDump variable.
				string xmpDump = xmp.Dump();

				// Test the schemas available in this release.
				// Some general comments:
				// To remove a property from XMP; Clear() arrays and set values to null.
				// Likewise, empty lists and null values did not exist in XMP.

				// The following schemas are defined in Adobe XMP Specification:
				// http://www.adobe.com/devnet/xmp/pdfs/xmp_specification.pdf
				TestDublinCore(xmp);
				TestExifAdditional(xmp);
				TestExifSpecific(xmp);
				TestExifTiff(xmp);
				TestPdf(xmp);
				TestPhotoshop(xmp);
				TestRaw(xmp);
				TestXmpBasic(xmp);
				TestXmpDynamic(xmp);
				TestXmpJob(xmp);
				TestXmpMedia(xmp);
				TestXmpRights(xmp);
				TestXmpText(xmp);

				// The following schema is defined in Image Map Specification:
				// http://www.halligang.se/xmp/imageMap-specification.pdf
				TestImageMap(xmp);

				// The following schema is defined in IPTC Specification:
				// http://www.iptc.org/std/Iptc4xmpCore/1.0/specification/Iptc4xmpCore_1.0-spec-XMPSchema_8.pdf
				TestIptc(xmp);

				// The following schema is defined in Personal Classification Specification:
				// http://www.halligang.se/xmp/pcs-specification.pdf
				TestPersonalClassification(xmp);

				// Uncomment the following line to save an empty XMP block.
				//xmp.Clear();

				// See what's about to be stored.
				// Set a breakpoint on the following line and
				// inspect the xmpDump variable.
				xmpDump = xmp.Dump();

				// Save XMP.
				// Uncomment the following line to save the XMP to file.
				//xmp.Save();
			}

			// If the Toolkit was manually initialized, it must also
			// be terminated. Uncomment the following line to do so:
			//Xmp.Terminate();
		}

		private static void TestDublinCore(Xmp xmp)
		{
			DublinCore dc = new DublinCore(xmp);
			dc.Source = "XmpSample project";
			dc.Subject.Clear();
			dc.Subject.Add("XmpSample");
			dc.Subject.Add("C# XMP Toolkit");
			dc.Subject.Insert(1, "demonstrates");
			dc.Title.Clear();
			dc.Title.DefaultValue = "C# XMP Toolkit sample";
			// The above is the same as:
			// dc.Title["x-default"] = "C# XMP Toolkit sample";
			dc.Title.Add(XmpLang.Swedish, "C# XMP Toolkit smakprov");
			// The above is the same as:
			// dc.Title.Add("sv-SE", "C# XMP Toolkit smakprov");
		}

		private static void TestExifAdditional(Xmp xmp)
		{
			ExifAdditional aux = new ExifAdditional(xmp);
			aux.SerialNumber = "1234567890";
		}

		private static void TestExifSpecific(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestExifTiff(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestPdf(Xmp xmp)
		{
			Pdf pdf = new Pdf(xmp);
			pdf.PdfVersion = "1.3";
			pdf.Producer = "XmpSample";
		}

		private static void TestPhotoshop(Xmp xmp)
		{
			Photoshop photoshop = new Photoshop(xmp);
			photoshop.Instructions = "Handle with care...";
		}

		private static void TestRaw(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestXmpBasic(Xmp xmp)
		{
			XmpBasic xmpBasic = new XmpBasic(xmp);
			xmpBasic.Identifier.Clear();
			xmpBasic.Identifier.Add(new XmpBasic.QualifiedIdentifier("Identifier A", null));
			xmpBasic.Identifier.Add(new XmpBasic.QualifiedIdentifier("Identifier B", "Scheme B"));
			xmpBasic.MetadataDate = DateTime.Now;
			xmpBasic.Rating = 5;
			xmpBasic.Thumbnails.Clear();
			// The following line is commented out to add readability to the XMP.
			// Uncomment it to verify that the thumbnail is stored.
			//xmpBasic.Thumbnails.Add(thumbnail);
		}

		private static void TestXmpDynamic(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestXmpJob(Xmp xmp)
		{
			XmpJob xmpBJ = new XmpJob(xmp);
			xmpBJ.JobRef.Clear();
			xmpBJ.JobRef.Add(new Job("Test of C# XMP Toolkit", "csxmptk", null));
		}

		private static void TestXmpMedia(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestXmpRights(Xmp xmp)
		{
			XmpRights xmpRights = new XmpRights(xmp);
			xmpRights.Owner.Clear();
			xmpRights.Owner.Add("Martin Sanneblad");
			xmpRights.UsageTerms.Clear();
			xmpRights.UsageTerms.DefaultValue = "Sample provided to test with C# XMP Toolkit.";
			// The above is the same as:
			// xmpRights.UsageTerms["x-default"] = "Sample provided to test with C# XMP Toolkit.";
		}

		private static void TestXmpText(Xmp xmp)
		{
			// Not yet implemented!
			// Please send a mail if this schema is required.
			// E-mail address is found at support page:
			// http://www.halligang.se/xmp/
		}

		private static void TestImageMap(Xmp xmp)
		{
			ImageMap imageMap = new ImageMap(xmp);
			imageMap.ImageSize.SetDimensions(1024, 768, "pixel");
			imageMap.Areas.Clear();
			imageMap.Areas.Add(new Area(
				ShapeType.Rectangle,
				new int[] { 100, 100, 200, 200 },
				new LangEntry[] { new LangEntry("x-default", "Martin") },
				null,
				null));
		}

		private static void TestIptc(Xmp xmp)
		{
			Iptc iptc4xmpCore = new Iptc(xmp);
			iptc4xmpCore.CopyrightNotice.Clear();
			iptc4xmpCore.CopyrightNotice.DefaultValue = "Copyright (c) 2007 Martin Sanneblad";
			// The above is the same as:
			// iptc4xmpCore.CopyrightNotice["x-default"] = "Copyright (c) 2007 Martin Sanneblad";
			iptc4xmpCore.CountryCode = "sv";
			iptc4xmpCore.CreatorContactInfo.Clear();
			iptc4xmpCore.CreatorContactInfo.WebUrl = "http://www.halligang.se/xmp/";
			iptc4xmpCore.CreatorContactInfo.Country = "Sweden";
			iptc4xmpCore.IntellectualGenre = "sample";			

			// IPTC Schema shares some properties with Dublin Core, Photoshop and XMP Rights schemas.
			// Please see the IPTC specification for information.
			// The following will actually add to the Photoshop schema:
			iptc4xmpCore.Country = "Sweden";
			iptc4xmpCore.DescriptionWriter = "Martin Sanneblad";
		}

		private static void TestPersonalClassification(Xmp xmp)
		{
			PersonalClassification pcs = new PersonalClassification(xmp);
			pcs.Creator = "Martin Sanneblad";
			pcs.Classifications.Clear();
			pcs.Classifications.Add(new Classification("Martin", ClassificationType.Descriptive, new string[] { "People", "Owners" }));
		}

		#endregion

		#region Helpers

		private static Thumbnail ThumbnailFromFile(string filePath)
		{
			Size thumnailSize = new Size(160, 120);
			byte[] imageBytes;
			using (Image original = Image.FromFile(filePath))
			{
				if (original.Width > original.Height)
				{
					thumnailSize.Height = (int)(original.Height / (float)original.Width * thumnailSize.Width);
				}
				else
				{
					thumnailSize.Width = (int)(original.Width / (float)original.Height * thumnailSize.Height);
				}

				using (Image thumbnail = new Bitmap(thumnailSize.Width, thumnailSize.Height, PixelFormat.Format24bppRgb))
				{
					using (Graphics graphics = Graphics.FromImage(thumbnail))
					{
						graphics.CompositingQuality = CompositingQuality.HighQuality;
						graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.DrawImage(original, new Rectangle(0, 0, thumbnail.Width, thumbnail.Height));
					}

					using (MemoryStream stream = new MemoryStream())
					{
						thumbnail.Save(stream, ImageFormat.Jpeg);
						stream.Position = 0;
						imageBytes = new byte[stream.Length];
						stream.Read(imageBytes, 0, imageBytes.Length);
					}
				}
			}

			string imageBase64 = Convert.ToBase64String(imageBytes, Base64FormattingOptions.InsertLineBreaks);
			return new Thumbnail(thumnailSize.Width, thumnailSize.Height, imageBase64);
		}

		private static string HandlerCapabilities()
		{
			string handlerCapabilities = string.Empty;

			// Get a list of handler capabilities.
			if (XmpCore.Initialize() && XmpFiles.Initialize())
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("File Format     : Scanning Mode : Can Insert");
				sb.AppendLine("--------------------------------------------");

				FileFormat[] fileFormats = (FileFormat[])Enum.GetValues(typeof(FileFormat));
				foreach (FileFormat fileFormat in fileFormats)
				{
					sb.Append(fileFormat.ToString().PadRight(16, ' '));
					sb.Append(": ");

					FileFormatInfo fileFormatInfo = new FileFormatInfo(fileFormat);
					if (fileFormatInfo.ScanningMode == ScanningMode.Smart)
					{
						sb.Append("Smart         : ");
					}
					else
					{
						sb.Append("Packet        : ");
					}

					if ((fileFormatInfo.FormatInfo & FormatInfo.CanInjectXMP) == FormatInfo.CanInjectXMP)
					{
						sb.AppendLine("True");
					}
					else
					{
						sb.AppendLine("False");
					}
				}

				handlerCapabilities = sb.ToString();

				XmpFiles.Terminate();
				XmpCore.Terminate();
			}

			return handlerCapabilities;
		}

		#endregion
	}
}
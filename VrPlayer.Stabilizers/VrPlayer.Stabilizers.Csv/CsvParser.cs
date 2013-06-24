using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;

namespace VrPlayer.Stabilizers.Csv
{
    public class CsvParser
    {
        private static readonly char[] Delimiters = new[] { ',' };

        public static IEnumerable<CsvFrame> Parse(string filePath)
        {
            var csvFrames = new List<CsvFrame>();

            using (var reader = new StreamReader(filePath))
            {
                var i = 0;
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    var parts = line.Split(Delimiters);

                    var frame = new CsvFrame
                    {
                        FrameNumber = int.Parse(parts[0]),
                        Translation = new Vector3D(
                            double.Parse(parts[1]),
                            double.Parse(parts[2]), 
                            double.Parse(parts[3])),
                        Rotation = new Quaternion(
                            double.Parse(parts[4]),
                            double.Parse(parts[5]),
                            double.Parse(parts[6]),
                            double.Parse(parts[7]))
                    };

                    csvFrames.Add(frame);
                }
            }

            return csvFrames;
        }
    }
}
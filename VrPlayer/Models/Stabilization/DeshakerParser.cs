using System.Collections.Generic;
using System.IO;

namespace VrPlayer.Models.Stabilization
{
    public class DeshakerParser
    {
        private static readonly char[] Delimiters = new[] { '\t' };

        public static IEnumerable<DeshakerFrame> Parse(string filePath)
        {
            var deshakerFrames = new List<DeshakerFrame>();

            using (var reader = new StreamReader(filePath))
            {
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    var parts = line.Split(Delimiters);

                    var frame = new DeshakerFrame
                    {
                        FrameNumber = int.Parse(parts[0]),
                        PanX = double.Parse(parts[1]),
                        PanY = double.Parse(parts[2]),
                        Rotation = double.Parse(parts[3]),
                        Zoom = double.Parse(parts[4])
                    };

                    deshakerFrames.Add(frame);
                }
            }

            return deshakerFrames;
        }
    }
}

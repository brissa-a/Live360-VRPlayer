using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Csv
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class CsvPlugin : PluginBase<IStabilizer>
    {
        public CsvPlugin()
        {
            Name = "CSV";
            var stabilizer = new CsvStabilizer();
            Content = stabilizer;
            Panel = new CsvPanel(stabilizer);
        }
    }
}

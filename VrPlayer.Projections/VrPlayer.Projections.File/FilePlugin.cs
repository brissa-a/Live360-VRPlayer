using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.File
{
    [Export(typeof(IPlugin<IProjection>))]
    public class FilePlugin : PluginBase<IProjection>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();
        
        public FilePlugin()
        {
            Name = "File";
            var projection = new FileProjection
            {
                FilePath = Config.AppSettings.Settings["DefaultFilePath"].Value
            };
            Content = projection;
            Panel = new FilePanel(projection);
        }
    }
}
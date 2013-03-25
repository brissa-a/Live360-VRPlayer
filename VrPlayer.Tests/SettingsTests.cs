using System.Collections.Generic;
using System.Configuration;
using System.Windows.Media.Effects;
using Moq;
using NUnit.Framework;

using VrPlayer.Models.Plugins;
using VrPlayer.Models.Settings;
using VrPlayer.Models.Shaders;
using VrPlayer.Models.State;
using VrPlayer.Models.Trackers;

namespace VrPlayer.Tests
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void CanLoadValueToSettings()
        {
            var stateMock = new Mock<IApplicationState>();            
            var settingsMock = new Mock<ApplicationSettingsBase>();
            var pluginsManagerMock = new Mock<IPluginManager>();

            pluginsManagerMock
                .Setup(mock => mock.Trackers)
                .Returns(new List<TrackerPlugin>
                    {
                        new TrackerPlugin(new KinectTracker(), ""),
                        new TrackerPlugin(new RazerHydraTracker(), "")
                    });

            var shaderMock = new Mock<ShaderEffect>();
            shaderMock
                .Setup(mock => mock.GetType())
                .Returns(typeof(PincushionEffect));
            pluginsManagerMock
                .Setup(mock => mock.Shaders)
                .Returns(new List<ShaderPlugin>
                    {
                        new ShaderPlugin(shaderMock.Object, "")
                    });

            settingsMock
                .Setup(mock => mock["Tracker"])
                .Returns("VrPlayer.Models.Trackers.RazerHydraTracker");
            settingsMock
                .Setup(mock => mock["Distortion"])
                .Returns("VrPlayer.Models.Shaders.PincushionEffect");

            var settingsManager = new SettingsManager(settingsMock.Object, stateMock.Object, pluginsManagerMock.Object);
            settingsManager.Load();

            Assert.That(stateMock.Object.TrackerPlugin.Tracker, Is.TypeOf<RazerHydraTracker>());
            Assert.That(stateMock.Object.ShaderPlugin.Shader, Is.TypeOf<PincushionEffect>());
        }
    }
}
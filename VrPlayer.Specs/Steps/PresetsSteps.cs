using System;
using System.IO;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.State;
using VrPlayer.Projections.Sphere;
using VrPlayer.ViewModels;

namespace VrPlayer.Specs.Steps
{
    [Binding]
    public class PresetsSteps
    {
        private readonly Mock<IApplicationState> _stateMock = new Mock<IApplicationState>();
        private readonly string _tempPresetFilePath = Path.GetTempPath() + "preset.json";

        [Given(@"my current projection is spherical")]
        public void GivenMyCurrentProjectionIsSpherical()
        {
            _stateMock
                .Setup(mock => mock.ProjectionPlugin)
                .Returns(new SpherePlugin());
        }

        [Given(@"my current stereo format is side by side")]
        public void GivenMyCurrentStereoFormatIsSideBySide()
        {
            _stateMock
                .Setup(mock => mock.StereoInput)
                .Returns(StereoMode.SideBySide);
        }
        
        [Given(@"there are no effects")]
        public void GivenThereAreNoEffects()
        {
            _stateMock
                .Setup(mock => mock.EffectPlugin)
                .Returns((IPlugin<EffectBase>) null);
        }
        
        [When(@"I press save from the preset menu")]
        public void WhenIPressSaveFromThePresetMenu()
        {
            var pluginManagerMock = new Mock<IPluginManager>();
            var configMock = new Mock<IApplicationConfig>();
            var presetManager = new PresetsManager(configMock.Object, _stateMock.Object, pluginManagerMock.Object);
            var viewModel = new MenuViewModel(_stateMock.Object, pluginManagerMock.Object, configMock.Object, presetManager);

            viewModel.SaveMediaPresetCommand.Execute(_tempPresetFilePath);
        }
        
        [Then(@"a file is created with the content of ""(.*)""")]
        public void ThenAFileIsCreatedWithTheContentOf(string filePath)
        {
            var expectedFileInfo = new FileInfo(filePath);
            var actualFileInfo = new FileInfo(_tempPresetFilePath);

            FileAssert.AreEqual(expectedFileInfo, actualFileInfo);
        }
    }
}
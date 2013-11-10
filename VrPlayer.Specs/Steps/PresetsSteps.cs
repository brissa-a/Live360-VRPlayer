using System;
using TechTalk.SpecFlow;

namespace VrPlayer.Specs.Steps
{
    [Binding]
    public class PresetsSteps
    {
        [Given(@"my current projection is spherical")]
        public void GivenMyCurrentProjectionIsSpherical()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"my current stereo format is side by side")]
        public void GivenMyCurrentStereoFormatIsSideBySide()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"there are no effects")]
        public void GivenThereAreNoEffects()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I press save from the preset menu")]
        public void WhenIPressSaveFromThePresetMenu()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"a file is created with the content of ""(.*)""")]
        public void ThenAFileIsCreatedWithTheContentOf(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
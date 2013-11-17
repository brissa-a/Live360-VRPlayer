Feature: Presets
	In order to avoid application configuration each time I load a media file
	As VR Player user
	I want to save and load preset files

Scenario: Save media preset
	Given my current projection is spherical
	And my current stereo format is side by side 
	And there are no effects
	When I press save from the preset menu
	Then a file is created with the content of "Presets/MediaPreset.json"

#Scenario: Load media preset
#Scenario: Save device preset
#Scenario: Load device preset
using Xunit.Gherkin.Quick;

namespace test;

[FeatureFile("D:\\RE\\backend\\BE\\test\\Features\\PsychologistLanguage.feature")]
public class PsychologistLanguageSteps : Feature
{
    [Given(@"the psychologist is on the settings page")]
    public void GivenThePsychologistIsOnTheSettingsPage()
    {
        // TODO: Simulate navigating to the settings page
    }

    [When(@"the psychologist changes the language")]
    public void WhenThePsychologistChangesTheLanguage()
    {
        // TODO: Simulate choosing a different language
    }

    [Then(@"the system renders the content in the selected language")]
    public void ThenTheSystemRendersTheContentInTheSelectedLanguage()
    {
        // TODO: Assert that the UI/text is updated to the selected language
    }
}
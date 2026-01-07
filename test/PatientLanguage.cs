using Xunit.Gherkin.Quick;

namespace test;

[FeatureFile("D:\\RE\\backend\\test\\Features\\PatientLanguage.feature")]
public class PatientLanguageSteps : Feature
{
    [Given(@"the patient is on the settings page")]
    public void GivenThePatientIsOnTheSettingsPage()
    {
        // TODO: Simulate navigating to the settings page
    }

    [When(@"the patient changes the language")]
    public void WhenThePatientChangesTheLanguage()
    {
        // TODO: Simulate choosing a different language
    }

    [Then(@"the system renders the content in the selected language")]
    public void ThenTheSystemRendersTheContentInTheSelectedLanguage()
    {
        // TODO: Assert that the UI/text is updated to the selected language
    }
}
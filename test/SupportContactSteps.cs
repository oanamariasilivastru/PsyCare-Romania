using Xunit.Gherkin.Quick;

namespace test;
[FeatureFile("D:\\RE\\backend\\test\\Features\\SupportContacts.feature")]
public class SupportContactsSteps : Feature
{
    [Given(@"the psychologist is on the patient list page")]
    public void GivenThePsychologistIsOnThePatientListPage()
    {
        // TODO: Navigate or simulate loading the patient list page
    }

    [When(@"the psychologist selects a patient")]
    public void WhenThePsychologistSelectsAPatient()
    {
        // TODO: Simulate selecting a patient from the list
    }

    [Then(@"the system displays its listed support contatcs")]
    public void ThenTheSystemDisplaysItsListedSupportContacts()
    {
        // TODO: Assert that the patient's support contacts are displayed
    }
}
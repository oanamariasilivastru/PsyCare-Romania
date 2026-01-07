using Xunit.Gherkin.Quick;

namespace test;
[FeatureFile("D:\\RE\\backend\\test\\Features\\Newsletter.feature")]
public sealed class NewsletterSteps : Feature
{
    [Given(@"the psychologist is on newsletter page")]
    public void GivenThePsychologistIsOnNewsletterPage()
    {
        // TODO: Simulate navigating to the newsletter page
    }

    [Then(@"the system displays a list of articles ordered by publication time")]
    public void ThenTheSystemDisplaysAListOfArticlesOrderedByPublicationTime()
    {
        // TODO: Assert that the displayed articles are sorted by publication time
    }
}
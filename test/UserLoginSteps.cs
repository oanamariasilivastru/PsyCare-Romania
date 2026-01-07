using Xunit.Gherkin.Quick;

[FeatureFile("D:\\RE\\backend\\test\\Features\\Login.feature")]
public sealed class UserLoginSteps : Feature
{
    [Given(@"the user is on the login page")]
    public void GivenUserIsOnLoginPage()
    {
        // Navigate to login page (or mock)
        Console.WriteLine("User navigates to login page");
    }

    [When(@"the user enters valid credentials")]
    public void WhenUserEntersValidCredentials()
    {
        // Perform login with valid credentials
        Console.WriteLine("User enters valid credentials");
    }

    [Then(@"the user should see the dashboard")]
    public void ThenUserSeesDashboard()
    {
        // Assert dashboard is shown
        Console.WriteLine("User sees dashboard");
    }

    [When(@"the user enters an incorrect password")]
    public void WhenUserEntersIncorrectPassword()
    {
        // Attempt login with wrong password
        Console.WriteLine("User enters incorrect password");
    }

    [Then(@"the user sees an error message")]
    public void ThenUserSeesErrorMessage()
    {
        // Assert error message shown
        Console.WriteLine("User sees error message");
    }
}
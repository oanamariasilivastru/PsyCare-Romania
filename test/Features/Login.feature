Feature: User login
  Users should be able to log in securely

  Scenario: Successful login
    Given the user is on the login page
    When the user enters valid credentials
    And the user presses login button
    Then the user should see the dashboard

  Scenario: Login fails with invalid password
    Given the user is on the login page
    And the user enters an incorrect password
    When the user presses login button
    Then the user sees an error message

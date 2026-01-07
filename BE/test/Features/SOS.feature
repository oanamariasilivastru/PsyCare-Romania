Feature: User SOS Button
  Users should be able to request emergency help in case of life threatening situation

  Scenario: SOS
    Given the user is on the main page
    When the user presses the SOS button
    Then the psychologist will be called

Feature: Psychologist Language
  The system should allow the psychologist to select desired language

  Scenario: Psychologist Language Changing
    Given the psychologist is on the settings page
    When the psychologist changes the language
    Then the system renders the content in the selected language
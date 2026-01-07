Feature: Break Reminders
  The system should remind the psychologist to take breaks regularly

  Scenario: Scheduling
    Given the psychologist is logged in
    And the take break feature is enabled
    When the configured break interval has passed
    Then the system displays a notification

Feature: Session Scheduling
  The system should allow the psychologist to schedule appointments

  Scenario: Scheduling
    Given the psychologist is on the calendar page
    When the psychologist clicks on an hour slot
    And the psychologist fill in the form field
    And the psychologist presses add button
    Then the system saves the appointment in calendar

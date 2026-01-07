Feature: Patient Chat
  The system should allow the patient to communicate with psychologist outside of sessions

  Scenario: Scheduling
    Given the patient is on the chat page
    When the patient enters the message
    And the patient presses send button
    Then the system sends the message to the psychologist

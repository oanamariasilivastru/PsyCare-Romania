Feature: Patient Language 
  The system should allow the patient to select desired language 
  
  Scenario: Patient Language Changing 
    Given the patient is on the settings page 
    When the patient changes the language 
    Then the system renders the content in the selected language
Feature: Support Contacts
  The psychologist should be able to see the patient support contacts

  Scenario: Support contacts
    Given the psychologist is on the patient list page
    When the psychologist selects a patient
    Then the system displays its listed support contatcs

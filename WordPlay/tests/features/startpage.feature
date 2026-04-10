Feature: Start page

Scenario: Host a game
  Given I am on the start page
  When I enter a username "David"
  And I click host game
  Then I should be redirected to rules page
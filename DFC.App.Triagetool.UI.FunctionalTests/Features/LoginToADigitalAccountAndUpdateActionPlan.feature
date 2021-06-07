Feature: Get personalised careers advice and information

Background:
	Given I am on the home page
	When I click the See suggestions button
	#Then I am taken to the Personalised careers advice and information page


@Triagetool @Smoke
Scenario: Change Triage options
	When I select planning or starting your career in the options filter
	When I select the Understand your skills filter
	Then I am shown the results for planning or starting your career
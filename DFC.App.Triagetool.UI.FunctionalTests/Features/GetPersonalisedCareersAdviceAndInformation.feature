Feature: Get personalised careers advice and information


@Triagetool @Smoke
Scenario Outline: Select Triage options
	#Given I am on the home page
	#When I click the See suggestions button
	Given I am on the triagetool page
	When I select <option> in the options filter
	Then I am shown the results for <option>
	And I am shown a result count of <results>

	Examples: 
	| option                           | results |
	| Identifying your skills          | 7       |
	| Identifying career opportunities | 7       |
	| Making career decisions          | 7       |
	| Finding and applying for jobs    | 15      |

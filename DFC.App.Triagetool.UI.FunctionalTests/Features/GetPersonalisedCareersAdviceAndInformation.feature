Feature: Get personalised careers advice and information


@Triagetool @Smoke
Scenario Outline: Select Triage options
	Given I am on the home page
	When I click the See suggestions button
	When I select <option> in the options filter
	Then I am shown the results for <option>
	And I am shown a result count of <results>

	Examples: 
	| option                                        | results |
	| planning or starting your career              | 28      |
	| moving up in your career                      | 31      |
	| changing your career                          | 23      |
	| returning to work                             | 33      |
	| understanding your options                    | 24      |
	| identifying and building your skills          | 21      |
	| understanding the recruitment process         | 12      |
	| working with a health condition or disability | 4       |
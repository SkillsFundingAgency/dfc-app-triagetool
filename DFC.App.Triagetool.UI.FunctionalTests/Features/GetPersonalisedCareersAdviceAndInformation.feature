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
	| option                                        | results |
	| Changing your career                          | 13      |
	| Identifying and building your skills          | 12      |
	| Moving up in your career                      | 10      |
	| Planning or starting your career              | 17      |
	| Returning to work                             | 15      |
	| Understanding the recruitment process         | 14      |
	| Understanding your options                    | 11      |
	| Working with a health condition or disability | 2       |


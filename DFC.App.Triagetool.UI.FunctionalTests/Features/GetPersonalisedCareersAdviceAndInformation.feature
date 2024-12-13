Feature: Get personalised careers advice and information


@Triagetool @Smoke
Scenario Outline: Select Triage options
	Given I am on the home page
	When I select <levelOne> from the list
	Then <levelTwo> is loaded for the selected <levelOne>
	#When I click the See suggestions button
	Given I am on the triagetool page
	When I select <option> in the options filter
	Then I am shown the results for <option>
	And I am shown a result count of <results>

	Examples: 
	| levelOne       | levelTwo                             | results |
	| in education   | in university						| 13      |
	| in education   | in college or sixth form				| 12      |
	| in education   | in secondary school					| 10      |
	| employed       | want to change career				| 17      |
	| employed       | want to progress in my career		| 15      |
	| employed       | at risk of redundancy				| 14      |
	| not in work    | want to return to work after a break | 11      |
	| not in work    | want to prepare to get a job			| 2		  |
	| not in work    | want to change career				| 2		  |


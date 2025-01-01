Feature: Get personalised careers advice and information


@Triagetool @Smoke
Scenario Outline: Verify two factor selection
	Given I am on the home page
	When I select <levelOne> from the list
	Then <levelTwo> is loaded for the selected <levelOne>

	Examples: 
	| levelOne       | levelTwo                                  |
	| in education   | in university							 |
	| in education   | in college or sixth form					 |
	| in education   | in secondary school						 |
	| employed       | want to change career					 |
	| employed       | want to progress in my career			 |
	| employed       | at risk of redundancy					 |
	| not in work    | want to return to work after a break 	 |
	| not in work    | want to prepare to get a job				 |
	| not in work    | want to change career					 |

Scenario Outline: Verify results for two factor selection
	Given I am on the home page
	When I choose <levelOne> and <levelTwo> from the list
	When I click on see advice button
	Then the result count should be <results>

	Examples: 
	| levelOne       | levelTwo                                  | results |
	| in education   | in university							 |   30    |
	| in education   | in college or sixth form					 |   28    |
	| in education   | in secondary school						 |   23    |  
	| employed       | want to change career					 |   25    |
	| employed       | want to progress in my career			 |   28    |
	| employed       | at risk of redundancy					 |   30    |
	| not in work    | want to return to work after a break 	 |   29    |
	| not in work    | want to prepare to get a job				 |   29    |
	| not in work    | want to change career					 |   29    |

Scenario Outline: Verify results for selected filters
	Given I am on the home page
	When I choose <levelOne> and <levelTwo> from the list
	When I click on see advice button
	And apply the filters

	Examples: 
	| levelOne       | levelTwo                                  | results |
	| in education   | in university							 |   30    |
	| in education   | in college or sixth form					 |   28    |
	| in education   | in secondary school						 |   23    |  
	| employed       | want to change career					 |   25    |
	| employed       | want to progress in my career			 |   28    |
	| employed       | at risk of redundancy					 |   30    |
	| not in work    | want to return to work after a break 	 |   29    |
	| not in work    | want to prepare to get a job				 |   29    |
	| not in work    | want to change career					 |   29    |

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
	And  I select the filters with the <filterOptions>
	And I click apply filters
	Then the result count should be <results>


	Examples: 
	| levelOne       | levelTwo                                  | filterOptions																																										| results | 
	| in education   | in university							 | Options for work,Developing or assessing your skills																																	|   10    |
	| in education   | in university							 | Options for work																																										|   6     |
	| in education   | in university							 | Options for work,Developing or assessing your skills,Disability support,CVs and applications,Further study options,Taking a gap year,Support you can get from others,Interview tips	|   30    |
	| in education   | in university							 | Options for work,,CVs and applications,Disability support																															|   14    |
	| in education   | in university							 | Further study options																																								|   3     |
	| in education   | in university							 | CVs and applications,Further study options,Taking a gap year																															|   11    |
	| in education   | in university							 | Support you can get from others,Interview tips																																		|   7     |
	| in education   | in college or sixth form					 | Further study options,Developing or assessing your skills,Options for work,Taking a gap year,CVs and applications,Support you can get from others,Interview tips,Disability support  |   28    |
	| in education   | in college or sixth form					 | CVs and applications,Support you can get from others,Interview tips,Disability support																								|   15    |
	| in education   | in college or sixth form					 | Further study options																																								|   2     |
	| in education   | in college or sixth form					 | Support you can get from others																																						|   2     |
	| in education   | in college or sixth form					 | Interview tips,Disability support																																					|   7     |
	| in education   | in college or sixth form					 | Taking a gap year,CVs and applications,Support you can get from others,Interview tips,Disability support																				|   17    |
	| in education   | in college or sixth form					 | Developing or assessing your skills,Options for work,Taking a gap year,CVs and applications																							|   17    |
	| in education   | in college or sixth form					 | Developing or assessing your skills,Options for work,Taking a gap year,CVs and applications,Support you can get from others,Interview tips,Disability support						|   26    |
	| in education   | in secondary school						 | Options after finishing school,Interview tips,Support you can get from others,Disability support,Developing or assessing your skills,CVs and applications							|   23    |  
	| in education   | in secondary school						 | Options after finishing school,Interview tips,Support you can get from others,Disability support																						|   13    |  
	| in education   | in secondary school						 | Options after finishing school,Interview tips																																		|   9     |  
	| in education   | in secondary school						 | Options after finishing school,Interview tips,Support you can get from others,Disability support,CVs and applications																|   18    |  
	| in education   | in secondary school						 | Support you can get from others,Disability support,Developing or assessing your skills,CVs and applications																			|   14    |  
	| in education   | in secondary school						 | Options after finishing school,Interview tips,Support you can get from others,Disability support,Developing or assessing your skills,CVs and applications							|   23    |  
	| in education   | in secondary school						 | CVs and applications																																									|   5     |  
	| in education   | in secondary school						 | Interview tips,Support you can get from others,Disability support,Developing or assessing your skills,CVs and applications															|   19    |  
	| in education   | in secondary school						 | Support you can get from others,Disability support,Developing or assessing your skills,CVs and applications																			|   14    |  
	| employed       | want to change career					 | Options for work																																										|   6     |
	| employed       | want to change career					 | Support you can get from others																																						|   2     |
	| employed       | want to change career					 | Options for work,CVs and applications,Developing or assessing your skills																											|   16    |
	| employed       | want to change career					 | Disability support,Interview tips,Support you can get from others																													|   9   |
	| employed       | want to change career					 | Options for work,CVs and applications,Developing or assessing your skills,Disability support,Interview tips																			|   23    |
	| employed       | want to change career					 | Options for work,CVs and applications,Developing or assessing your skills,Support you can get from others																			|   18    |
	| employed       | want to change career					 | Options for work,CVs and applications,Developing or assessing your skills,Disability support,Interview tips,Support you can get from others											|   25    |
	| employed       | want to change career					 | Options for work,CVs and applications,Disability support,Interview tips,Support you can get from others																				|   21    |
	| employed       | want to change career					 | Options for work,Developing or assessing your skills,Disability support,Interview tips,Support you can get from others																|   19    |
	| employed       | want to change career					 | CVs and applications,Developing or assessing your skills,Disability support,Interview tips,Support you can get from others															|   19    |
	| employed       | want to change career					 | Interview tips  |   5    |
	| employed       | want to change career					 | Developing or assessing your skills  |   4    |
	| employed       | want to change career					 | CVs and applications,Support you can get from others  |   8    |
	| employed       | want to change career					 | Options for work,Interview tips,Support you can get from others  |   13    |
	| employed       | want to progress in my career			 | Options for work,Developing or assessing your skills  |   13    |
	| employed       | at risk of redundancy					 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   30    |
	| employed       | at risk of redundancy					 | Options for work,Developing or assessing your skills |   14    |
	| employed       | at risk of redundancy					 | Options for work,Developing or assessing your skills,Support you can get from others  |   16    |
	| employed       | at risk of redundancy					 | CVs and applications,Interview tips,Disability support  |   14    |
	| employed       | at risk of redundancy					 | Support you can get from others,CVs and applications,Interview tips,Disability support  |   16    |
	| employed       | at risk of redundancy					 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   30    |
	| employed       | at risk of redundancy					 | Developing or assessing your skills,Support you can get from others  |   7    |
	| employed       | at risk of redundancy					 | Interview tips  |   5    |
	| employed       | at risk of redundancy					 | Disability support  |   2    |
	| not in work    | want to return to work after a break		 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to return to work after a break		 | Options for work,Developing or assessing your skills |   13    |
	| not in work    | want to return to work after a break		 | Options for work,Developing or assessing your skills,Support you can get from others  |   15    |
	| not in work    | want to return to work after a break		 | CVs and applications,Interview tips,Disability support  |   14    |
	| not in work    | want to return to work after a break		 | Support you can get from others,CVs and applications,Interview tips,Disability support  |   16    |
	| not in work    | want to return to work after a break		 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to return to work after a break		 | Developing or assessing your skills,Support you can get from others  |   7    |
	| not in work    | want to return to work after a break		 | Interview tips  |   5    |
	| not in work    | want to return to work after a break		 | Disability support  |   2    |
	| not in work    | want to prepare to get a job	 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to prepare to get a job	 | Options for work,Developing or assessing your skills |   13    |
	| not in work    | want to prepare to get a job	 | Options for work,Developing or assessing your skills,Support you can get from others  |   15    |
	| not in work    | want to prepare to get a job	 | CVs and applications,Interview tips,Disability support  |   14    |
	| not in work    | want to prepare to get a job	 | Support you can get from others,CVs and applications,Interview tips,Disability support  |   16    |
	| not in work    | want to prepare to get a job	 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to prepare to get a job	 | Developing or assessing your skills,Support you can get from others  |   7    |
	| not in work    | want to prepare to get a job	 | Interview tips  |   5    |
	| not in work    | want to prepare to get a job	 | Disability support  |   2    |
	| not in work    | want to change career	 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to change career	 | Options for work,Developing or assessing your skills |   13    |
	| not in work    | want to change career	 | Options for work,Developing or assessing your skills,Support you can get from others  |   15    |
	| not in work    | want to change career	 | CVs and applications,Interview tips,Disability support  |   14    |
	| not in work    | want to change career	 | Support you can get from others,CVs and applications,Interview tips,Disability support  |   16    |
	| not in work    | want to change career	 | Options for work,Developing or assessing your skills,Support you can get from others,CVs and applications,Interview tips,Disability support  |   29    |
	| not in work    | want to change career	 | Developing or assessing your skills,Support you can get from others  |   7    |
	| not in work    | want to change career	 | Interview tips  |   5    |
	| not in work    | want to change career	 | Disability support  |   2    |

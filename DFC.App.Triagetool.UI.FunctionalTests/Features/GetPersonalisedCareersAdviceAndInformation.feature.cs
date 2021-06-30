﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.4.0.0
//      SpecFlow Generator Version:3.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace DFC.App.Triagetool.UI.FunctionalTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GetPersonalisedCareersAdviceAndInformationFeature : object, Xunit.IClassFixture<GetPersonalisedCareersAdviceAndInformationFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "GetPersonalisedCareersAdviceAndInformation.feature"
#line hidden
        
        public GetPersonalisedCareersAdviceAndInformationFeature(GetPersonalisedCareersAdviceAndInformationFeature.FixtureData fixtureData, DFC_App_Triagetool_UI_FunctionalTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-GB"), "Features", "Get personalised careers advice and information", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        public virtual void SelectTriageOptions(string option, string results, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Triagetool",
                    "Smoke"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            string[] tagsOfScenario = @__tags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("option", option);
            argumentsOfScenario.Add("results", results);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Select Triage options", null, tagsOfScenario, argumentsOfScenario);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
 testRunner.Given("I am on the home page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 7
 testRunner.When("I click the See suggestions button", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 8
 testRunner.When(string.Format("I select {0} in the options filter", option), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 9
 testRunner.Then(string.Format("I am shown the results for {0}", option), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 10
 testRunner.And(string.Format("I am shown a result count of {0}", results), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: planning or starting your career")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: planning or starting your career")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_PlanningOrStartingYourCareer()
        {
#line 5
this.SelectTriageOptions("planning or starting your career", "28", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: moving up in your career")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: moving up in your career")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_MovingUpInYourCareer()
        {
#line 5
this.SelectTriageOptions("moving up in your career", "31", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: changing your career")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: changing your career")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_ChangingYourCareer()
        {
#line 5
this.SelectTriageOptions("changing your career", "23", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: returning to work")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: returning to work")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_ReturningToWork()
        {
#line 5
this.SelectTriageOptions("returning to work", "33", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: understanding your options")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: understanding your options")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_UnderstandingYourOptions()
        {
#line 5
this.SelectTriageOptions("understanding your options", "24", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: identifying and building your skills")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: identifying and building your skills")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_IdentifyingAndBuildingYourSkills()
        {
#line 5
this.SelectTriageOptions("identifying and building your skills", "21", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: understanding the recruitment process")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: understanding the recruitment process")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_UnderstandingTheRecruitmentProcess()
        {
#line 5
this.SelectTriageOptions("understanding the recruitment process", "12", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: working with a health condition or disability")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: working with a health condition or disability")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_WorkingWithAHealthConditionOrDisability()
        {
#line 5
this.SelectTriageOptions("working with a health condition or disability", "4", ((string[])(null)));
#line hidden
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GetPersonalisedCareersAdviceAndInformationFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GetPersonalisedCareersAdviceAndInformationFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion

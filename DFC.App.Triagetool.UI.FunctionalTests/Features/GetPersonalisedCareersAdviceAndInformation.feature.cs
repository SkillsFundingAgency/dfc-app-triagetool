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
#line 8
 testRunner.Given("I am on the triagetool page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 9
 testRunner.When(string.Format("I select {0} in the options filter", option), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 10
 testRunner.Then(string.Format("I am shown the results for {0}", option), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 11
 testRunner.And(string.Format("I am shown a result count of {0}", results), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: Identifying your skills")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: Identifying your skills")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_IdentifyingYourSkills()
        {
#line 5
this.SelectTriageOptions("Identifying your skills", "7", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: Identifying career opportunities")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: Identifying career opportunities")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_IdentifyingCareerOpportunities()
        {
#line 5
this.SelectTriageOptions("Identifying career opportunities", "7", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: Making career decisions")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: Making career decisions")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_MakingCareerDecisions()
        {
#line 5
this.SelectTriageOptions("Making career decisions", "7", ((string[])(null)));
#line hidden
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Select Triage options: Finding and applying for jobs")]
        [Xunit.TraitAttribute("FeatureTitle", "Get personalised careers advice and information")]
        [Xunit.TraitAttribute("Description", "Select Triage options: Finding and applying for jobs")]
        [Xunit.TraitAttribute("Category", "Triagetool")]
        [Xunit.TraitAttribute("Category", "Smoke")]
        public virtual void SelectTriageOptions_FindingAndApplyingForJobs()
        {
#line 5
this.SelectTriageOptions("Finding and applying for jobs", "15", ((string[])(null)));
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

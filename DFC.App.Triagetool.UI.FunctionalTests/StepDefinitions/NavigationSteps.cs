// <copyright file="NavigationSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Triagetool.Model;
using DFC.App.Triagetool.UI.FunctionalTests.Pages;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System;
using System.Globalization;
using System.Threading;
using TechTalk.SpecFlow;

namespace DFC.App.Triagetool.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class NavigationSteps
    {
        public NavigationSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [Then(@"I am on the (.*) page")]
        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnThePage(string pageName)
        {
            var pageHeadingLocator = By.ClassName("govuk-heading-xl");

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "home":
                    var homePage = new TriagetoolPage(this.Context);
                    homePage.NavigateToHomepage();
                    Thread.Sleep(4000);
                    break;

                case "triagetool":
                    var triagetoolPage = new TriagetoolPage(this.Context);
                    triagetoolPage.NavigateToTriagetoolPage();
                    this.Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(pageHeadingLocator, "Personalised careers advice and information");
                    break;

                default:
                    throw new OperationCanceledException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The page name provided was not recognised.");
            }
        }
    }
}

// <copyright file="ValidationSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Triagetool.Model;
using DFC.TestAutomation.UI.Extension;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace DFC.App.Triagetool.UI.FunctionalTests
{
    [Binding]
    internal class ValidationSteps
    {
        public ValidationSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [Then(@"(.*) is loaded for the selected (.*)")]
        public void ThenLevelTwoIsLoadedForTheSelectedLevelOne(string levelTwo, string levelOne)
        {
            Thread.Sleep(3000);
            var levelTwoSelect = this.Context.GetWebDriver().FindElement(By.Id("triageLevelTwo"));
            var selectElement = new SelectElement(levelTwoSelect);
            selectElement.Options.Any(x => x.GetAttribute("value") == levelTwo).Should().BeTrue();
            Thread.Sleep(1000);
        }

        [Then(@"I am taken to the (.*) page")]
        public void ThenIAmTakenToThePage(string pageName)
        {
            By locator = null;

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "Personalised careers advice and information":
                    locator = By.ClassName("dfc-app-triage-panel-banner");
                    break;
                default:
                    locator = By.CssSelector("h1.govuk-fieldset__heading");
                    break;
            }

            this.Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(locator, pageName);
        }

        [Then(@"I am shown the results for (.*)")]
        public void ThenIAmShownTheResultsForTheOption(string option)
        {
            var result = this.Context.GetWebDriver().FindElement(By.Id("primaryFiltersSelectedValue")).GetAttribute("innerText").ToString().ToLower();

            if (result != option.ToLower())
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The expected result is not displayed");
            }
        }

        [Then(@"the result count should be (.*)")]
        public void ThenIAmShownAResultCountOf(string count)
        {
            var result = this.Context.GetWebDriver().FindElement(By.CssSelector(".dfc-app-triage-page-number-panel p")).GetAttribute("innerText").ToString();
            result.Should().Be($"Showing {count} career resources");
        }
    }
}
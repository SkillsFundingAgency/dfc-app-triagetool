// <copyright file="BasicSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace DFC.App.Triagetool.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class BasicSteps
    {
        public BasicSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [When(@"I click the (.*) button")]
        public void WhenIClickTheButton(string buttonText)
        {
            var allbuttons = this.Context.GetWebDriver().FindElements(By.ClassName("govuk-button")).ToList();

            foreach (var button in allbuttons)
            {
                if (button.Text.Trim().Equals(buttonText, System.StringComparison.OrdinalIgnoreCase))
                {
                    button.Click();
                    return;
                }
            }

            throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The button could not be found.");
        }

        [When(@"I select (.*) in the options filter")]
        public void WhenISelectSortFilter(string options)
        {
            var optionsFilter = this.Context.GetWebDriver().FindElement(By.Id("triageSelect")).Click();

            if (!optionsFilter.Displayed)
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The sort by filter could not be located.");
            }

            var selectElement = new SelectElement(optionsFilter);
            selectElement.SelectByValue(options);
            optionsFilter.SendKeys(Keys.Tab);
            Thread.Sleep(1000);
        }

        [When(@"I select the (.*) filter")]
        public void WhenISelectCourseHoursFilter(string courseHours)
        {
            switch (courseHours)
            {
                case "Understand your skills":
                    this.Context.GetWebDriver().FindElement(By.Id("Understand your skills")).Click();
                    break;
                default:
                    throw new OperationCanceledException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The Course hours not listed.");
            }

            Thread.Sleep(5000);
        }
    }
}
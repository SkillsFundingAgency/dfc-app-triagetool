﻿// <copyright file="ContactUsLandingPage.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.Triagetool.Model;
using DFC.TestAutomation.UI.Extension;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.Triagetool.UI.FunctionalTests.Pages
{
    internal class TriagetoolPage
    {
        public TriagetoolPage(ScenarioContext context)
        {
            this.Context = context;

            if (this.Context == null)
            {
                throw new NullReferenceException("The scenario context is null. The action plans landing page cannot be initialised.");
            }
        }

        private ScenarioContext Context { get; set; }

        public TriagetoolPage NavigateToTriagetoolPage()
        {
            this.Context.GetWebDriver().Url = this.Context.GetSettingsLibrary<AppSettings>().AppSettings.AppBaseUrl.ToString() + "triagetool";
            return this;
        }

        public TriagetoolPage NavigateToHomepage()
        {
            this.Context.GetWebDriver().Url = this.Context.GetSettingsLibrary<AppSettings>().AppSettings.AppBaseUrl.ToString() + "home";
            return this;
        }
    }
}

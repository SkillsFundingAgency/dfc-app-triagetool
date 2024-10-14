using NHibernate.Mapping;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolFilterViewModel
    {
        public string? LevelOne { get; set; }

        public string? LevelTwo { get; set; }

        public string? FilterAdviceGroupOptions { get; set; }

        public TriageToolFilerAction FilterAction { get; set; }
    }
}

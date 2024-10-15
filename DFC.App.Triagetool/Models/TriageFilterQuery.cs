using System.Collections.Generic;

namespace DFC.App.Triagetool.Models
{
    public class TriageFilterQuery
    {
        public string? LevelOne { get; set; }

        public string? LevelTwo { get; set; }

        public List<string>? FilterAdviceGroup { get; set; }
    }
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class HealthViewModel
    {
        public IList<HealthItemViewModel>? HealthItems { get; set; }
    }
}

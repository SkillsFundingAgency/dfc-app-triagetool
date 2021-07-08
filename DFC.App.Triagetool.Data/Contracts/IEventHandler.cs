using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Data.Contracts
{
    public interface IEventHandler
    {
        public string ProcessType { get; }

        Task<HttpStatusCode> ProcessContentAsync(Uri url);

        Task<HttpStatusCode> DeleteContentAsync(Guid contentId);
    }
}

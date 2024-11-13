using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Helpers;
using source_service.Model;

namespace source_service.Repository.Interface
{
    public interface ISourceRepository
    {
        Task<IEnumerable<Source>> GetSources(QueryObjectSource query);
        Task<Source> GetSource(string id);
        Task<Source> AddSource(Source source);
        Task<Source> UpdateSource(Source source);
        Task<Source> DeleteSource(string id);

        Task<int> CountSources();
        Task<IEnumerable<Source>> DeleteSourcesByUserId(string userId);

        Task<IEnumerable<Source>> GetSourcesByUserId(string userId);


    }
}
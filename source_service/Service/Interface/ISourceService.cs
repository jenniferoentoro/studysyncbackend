using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Helpers;
using source_service.Model;

namespace source_service.Service.Interface
{
    public interface ISourceService
    {
        public Task<IEnumerable<Source>> GetSources(QueryObjectSource query);
        public Task<Source> GetSource(string id);
        public Task<Source> AddSource(Source source);
        public Task<Source> UpdateSource(Source source);
        public Task<Source> DeleteSource(string id);

        public Task<int> CountSources();

        Task<IEnumerable<Source>> DeleteSourcesByUserId(string userId);

        Task<IEnumerable<Source>> GetSourcesByUserId(string userId);


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using source_service.Helpers;
using source_service.Model;
using source_service.Repository.Interface;
using source_service.Service.Interface;

namespace source_service.Service
{
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _sourceRepository;

        public SourceService(ISourceRepository sourceRepository)
        {
            _sourceRepository = sourceRepository;
        }
        public async Task<Source> AddSource(Source source)
        {
            return await _sourceRepository.AddSource(source);
        }

        public async Task<Source> DeleteSource(string id)
        {
            return await _sourceRepository.DeleteSource(id);
        }

        public async Task<Source> GetSource(string id)
        {
            return await _sourceRepository.GetSource(id);
        }

        public async Task<IEnumerable<Source>> GetSources(QueryObjectSource query)
        {
            return await _sourceRepository.GetSources(query);
        }

        public async Task<Source> UpdateSource(Source source)
        {
            return await _sourceRepository.UpdateSource(source);
        }

        public async Task<int> CountSources()
        {
            return await _sourceRepository.CountSources();
        }

        public async Task<IEnumerable<Source>> DeleteSourcesByUserId(string userId)
        {
            return await _sourceRepository.DeleteSourcesByUserId(userId);
        }

        public async Task<IEnumerable<Source>> GetSourcesByUserId(string userId)
        {
            return await _sourceRepository.GetSourcesByUserId(userId);
        }
    }
}
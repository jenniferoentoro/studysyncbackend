using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Model
{
    public class DatabaseConfiguration
    {

        public string ConnectionString { get; set; }

        public string CategoriesCollectionName { get; set; }

        public string DatabaseNameSource { get; set; }

        public string SourcesCollectionName { get; set; }

        public string FileStorageCollectionName { get; set; }
    }
}
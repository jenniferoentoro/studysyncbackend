using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Model
{
    public class UserDatabaseConfiguration
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
    }
}
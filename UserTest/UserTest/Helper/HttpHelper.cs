using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserTest.Helper
{
    internal class HttpHelper
    {
        internal static class Urls
        {
            public readonly static string getAllUsers = "/all";
            public readonly static string user = "/api/users/{0}";
        }

    }
}
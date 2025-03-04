using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace user_service.Dtos
{
    public class ResponseModel
    {
        public string kind { get; set; }
        public string localId { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public string idToken { get; set; }
        public bool registered { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
    }
}
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConfigurationModels
{
    public class JwtConfiguration
    {
        public string Section { get; set; } = Constants.SETTINGS_JWT_KEY;
        public string? ValidIssuer { get; set; }
        public string? ValidAudience { get; set; }
        public string? Expires { get; set; }
    }
}

using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConfigurationModels
{
    public class GoogleConfiguration
    {
        public static string Section { get; } = Constants.SETTINGS_GOOGLE_KEY;
        public string? ClientId { get; set; }
    }
}

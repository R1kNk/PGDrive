using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace PGoogleDrive
{
    class ConfigModifyMethods
    {
        static void AddPGDriveConfigSection()
        {
            Configuration config;
            config = WebConfigurationManager.OpenWebConfiguration("~");
            PGDriveConfigsSection appsettings;
            appsettings = (PGDriveConfigsSection)config.GetSection("pGDriveConfigs");
            PGDriveElementCollection drives = appsettings.GDrives;
        }
    }
}

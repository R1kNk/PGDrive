using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGoogleDrive.Internal
{
    class InternalMethods
    {

        public static DriveService GetDriveServicev3(UserCredential credential, string ApplicationName)
        {
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            }
                );

        }

        public static Google.Apis.Drive.v2.DriveService GetDriveServicev2(UserCredential credential, string ApplicationName)
        {
            return new Google.Apis.Drive.v2.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            }
                );

        }


      

        public static Permission SetFilePermission(string fileId, DriveService service ,Permission permission)
        {
            var existPermission = GetFilePermissions(fileId, service).Any(p => p.Role == permission.Role && p.Type == permission.Type);
            if (existPermission == false)
            {
                var result = service.Permissions.Create(permission, fileId).Execute();
                return result;
            }
            return null;
        }

        public static bool DeleteFilePermission(string fileId, DriveService service, Permission permission)
        {
            var existPermission = GetFilePermissions(fileId, service).Where(p => p.Role == permission.Role && p.Type == permission.Type).SingleOrDefault();
            if (existPermission !=null)
            {
                var result = service.Permissions.Delete(fileId, existPermission.Id).Execute();
                return true;
            }
            return false;
        }

        public static IEnumerable<Permission> GetFilePermissions(string fileId, DriveService service)
        {
            var result = service.Permissions.List(fileId).Execute();
            return result.Permissions;
        }
        


    }
}

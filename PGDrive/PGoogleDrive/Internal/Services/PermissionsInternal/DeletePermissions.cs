using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models.General;
using PGoogleDrive.Internal.Services.PermissionsInternal.Models;
using System.Linq;
using static PGoogleDrive.Internal.Services.Permissions;

namespace PGoogleDrive.Internal.Services.PermissionsInternal
{
    class DeletePermissions : DeletePermissionsModule
    {
        public DeletePermissions(DriveService service) : base(service)
        {
        }

        protected override PGDriveResult<bool> DeleteFilePermission(string fileId, DriveService service, Permission permission)
        {
            var resultPermissions = GetFilePermissions(fileId, service);
            if (resultPermissions.isSuccess == false) { return resultPermissions.CopyResult<bool>(); }

            var existPermission = resultPermissions.ResponseBody.Where(p => p.Role == permission.Role && p.Type == permission.Type).ToList();
            if(permission.EmailAddress!=null) { existPermission = existPermission.Where(p => p.EmailAddress == permission.EmailAddress).ToList(); }
            else if(permission.Domain!=null) { existPermission = existPermission.Where(p => p.Domain == permission.Domain).ToList(); }
            Permission findedPermission = new Permission();
            PGDriveResult<bool> pGDriveResult = new PGDriveResult<bool>();
            if (existPermission.Count != 1)
            {
                pGDriveResult.SetResponseBody(false);
                return pGDriveResult;
            }
                try
                {
                if (findedPermission != null)
                {
                    var result = service.Permissions.Delete(fileId, existPermission.SingleOrDefault().Id).Execute();
                    pGDriveResult.SetResponseBody(true);
                    return pGDriveResult;
                }
                pGDriveResult.SetResponseBody(false);
                return pGDriveResult;
            }
            catch (GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                pGDriveResult.SetResponseBody(false);
                return pGDriveResult;
            }

        }

        protected override PGDriveResult<bool> DeleteFilePermission(string fileId, Types type, Roles role, string email = null, string domain = null)
        {
            Permission newPermission = CreatePermission((int)type, (int)role);
            if (email != null) newPermission.EmailAddress = email;
            if (domain != null) newPermission.Domain = domain;
            var result = DeleteFilePermission(fileId, driveService, newPermission);
            return result;
        }

    }
}

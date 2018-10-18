using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models;
using PGoogleDrive.Internal.Services.PermissionsInternal.Models;
using System.Linq;
using static PGoogleDrive.Internal.Services.Permissions;

namespace PGoogleDrive.Internal.Services.PermissionsInternal
{
    class CreatePermissions : CreatePermissionsModule
    {
        public CreatePermissions(DriveService service) : base(service)
        {
        }

        protected override PGDriveResult<Permission> CreateFilePermission(string fileId, DriveService service, Permission permission)
        {
            var resultPermissions = GetFilePermissions(fileId, service);
            if (resultPermissions.isSuccess == false) { return resultPermissions.CopyResult<Permission>(); }
            //
            var existPermission = resultPermissions.ResponseBody.Any(p => p.Role == permission.Role && p.Type == permission.Type);
            PGDriveResult<Permission> pGDriveResult = new PGDriveResult<Permission>();
            try
            {
                var result = service.Permissions.Create(permission, fileId);
                result.Fields= "id, emailAddress, domain, type, role, displayName, kind";
                var Request = result.Execute();
                    pGDriveResult.SetResponseBody(Request);
                    return pGDriveResult;
            }
            catch (GoogleApiException exception)
            {
                pGDriveResult.InitializeError(exception);
                return pGDriveResult;
            }
        }

        protected override PGDriveResult<Permission>  CreateFilePermission(string fileId, Types type, Roles role, string email = null, string domain = null)
        {
            Permission newPermission = CreatePermission((int)type, (int)role);
            if (email != null) newPermission.EmailAddress = email;
            else if (domain != null) newPermission.Domain = domain;
            var result = CreateFilePermission(fileId, driveService, newPermission);
            return result;
        }

    }
}

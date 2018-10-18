using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models;
using static PGoogleDrive.Internal.Services.Permissions;

namespace PGoogleDrive.Internal.Services.PermissionsInternal.Models
{
    public abstract class DeletePermissionsModule : ServiceModule
    {
        public DeletePermissionsModule(DriveService service) : base(service)
        {
        }

        protected abstract PGDriveResult<bool> DeleteFilePermission(string fileId, DriveService service, Permission permission);

        protected abstract PGDriveResult<bool> DeleteFilePermission(string fileId, Types type, Roles role, string email = null, string domain = null);

        /// <summary>
        /// Deletes permission of file using Permission object itself
        /// </summary>
        /// <param name="Id">Id of file or folder to delete permission from</param>
        /// <param name="permission">Object of permission itself</param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermission(string Id, Permission permission)
        {
            var result = DeleteFilePermission(Id, driveService, permission);
            return result;
        }

        /// <summary>
        /// Deletes permission from a  file or a folder of a specific user
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from a specific user</param>
        /// <param name="userEmail">Email of user which have that permission/param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionUser(string fileOfFolderId, Roles role, string userEmailAddress)
        {
            var result = DeleteFilePermission(fileOfFolderId, Types.user, role, userEmailAddress);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder of a specific google group
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from a specific google group</param>
        /// <param name="groupEmailAddress">Email of google group which have that permission/param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionGroup(string fileOfFolderId, Roles role, string groupEmailAddress)
        {
            var result = DeleteFilePermission(fileOfFolderId, Types.group, role, groupEmailAddress);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder of a specific domain
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from a specific google group</param>
        /// <param name="domain">Email of google group which have that permission/param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionDomain(string fileOfFolderId, Roles role, string domain)
        {
            var result = DeleteFilePermission(fileOfFolderId, Types.domain, role, domain);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder from all users
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from all users</param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionAnyone(string fileOfFolderId, Roles role)
        {
            var result = DeleteFilePermission(fileOfFolderId, Types.anyone, role);
            return result;
        }
    }
}

﻿using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models.General;
using static PGoogleDrive.Internal.Services.Permissions;

namespace PGoogleDrive.Internal.Models.Permissions
{
    /// <summary>
    /// Module that represents methods of deleting permissions
    /// </summary>
    public abstract class DeletePermissionsModule : ServiceModule
    {
        /// <summary>
        /// Initializes module with service
        /// </summary>
        /// <param name="service"></param>
        public DeletePermissionsModule(DriveService service) : base(service)
        {
        }

        /// <summary>
        /// Method must implement deleting of file permission in general
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="service"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        protected abstract PGDriveResult<bool> DeleteFilePermission(string fileId, DriveService service, Permission permission);
        /// <summary>
        /// Method must implement deleting of the file permission in more friendly way
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="type"></param>
        /// <param name="role"></param>
        /// <param name="email"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
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
        public PGDriveResult<bool> DeletePermissionUser(string fileOrFolderId, Roles role, string userEmailAddress)
        {
            var result = DeleteFilePermission(fileOrFolderId, Types.user, role, userEmailAddress);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder of a specific google group
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from a specific google group</param>
        /// <param name="groupEmailAddress">Email of google group which have that permission/param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionGroup(string fileOrFolderId, Roles role, string groupEmailAddress)
        {
            var result = DeleteFilePermission(fileOrFolderId, Types.group, role, groupEmailAddress);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder of a specific domain
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from a specific google group</param>
        /// <param name="domain">Email of google group which have that permission/param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionDomain(string fileOrFolderId, Roles role, string domain)
        {
            var result = DeleteFilePermission(fileOrFolderId, Types.domain, role, domain);
            return result;
        }
        /// <summary>
        /// Deletes permission from a file or a folder from all users
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder from which you want to delete permission</param>
        /// <param name="role">Role which you want to delete from all users</param>
        ///<returns>PGDriveResult with bool in ResponseBody which shows did permission was deleted or not</returns>
        public PGDriveResult<bool> DeletePermissionAnyone(string fileOrFolderId, Roles role)
        {
            var result = DeleteFilePermission(fileOrFolderId, Types.anyone, role);
            return result;
        }
    }
}

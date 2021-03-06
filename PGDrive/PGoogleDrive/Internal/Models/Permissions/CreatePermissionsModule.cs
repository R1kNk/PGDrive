﻿using Google.Apis.Drive.v3.Data;
using Google.Apis.Drive.v3;
using PGoogleDrive.Internal.Models.General;
using static PGoogleDrive.Internal.Services.Permissions;

namespace PGoogleDrive.Internal.Models.Permissions
{
    /// <summary>
    /// Module that represents methods of creating permissions
    /// </summary>
    public abstract class CreatePermissionsModule : ServiceModule
    {
        /// <summary>
        /// Initializes module with service
        /// </summary>
        /// <param name="service">Drive service to initialize</param>
        public CreatePermissionsModule(DriveService service) : base(service)
        {
        }

        /// <summary>
        /// Method that must implement creating of file permission in general
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="service"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        protected abstract PGDriveResult<Permission> CreateFilePermission(string Id, DriveService service, Permission permission);

        /// <summary>
        /// Method that must implement creating of file permission in more friendly way
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="type"></param>
        /// <param name="role"></param>
        /// <param name="email"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        protected abstract PGDriveResult<Permission> CreateFilePermission(string Id, Types type, Roles role, string email = null, string domain = null);

        /// <summary>
        /// Adds permission to file or a folder to a specific user
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder to which you want to add permission</param>
        /// <param name="role">Role which you want to add to a specific user for a file or folder</param>
        /// <param name="userEmail">Email of user to which you want to give that permission</param>
        ///<returns>PGDriveResult with created permission in responseBody</returns>
        public PGDriveResult<Permission> AddPermissionUser(string fileOrFolderId, Roles role, string userEmail)
        {
            var result = CreateFilePermission(fileOrFolderId, Types.user, role, email: userEmail);
            return result;
        }
        /// <summary>
        /// Adds permission to file or a folder to a specific google group
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder to which you want to add permission</param>
        /// <param name="role">Role which you want to add to a specific user for a file or folder</param>
        /// <param name="groupEmail">Email of google group to which you want to give that permission</param>
        ///<returns>PGDriveResult with created permission in responseBody</returns>
        public PGDriveResult<Permission> AddPermissionGroup(string fileOrFolderId, Roles role, string groupEmail)
        {
            var result = CreateFilePermission(fileOrFolderId, Types.group, role, email: groupEmail);
            return result;
        }
        /// <summary>
        /// Adds permission to file or a folder to a specific domain
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder to which you want to add permission</param>
        /// <param name="role">Role which you want to add to a specific user for a file or folder</param>
        /// <param name="domain">domain which you want to give that permission</param>
        ///<returns>PGDriveResult with created permission in responseBody</returns>
        public PGDriveResult<Permission> AddPermissionDomain(string fileOrFolderId, Roles role, string domain)
        {

            var result = CreateFilePermission(fileOrFolderId, Types.domain, role, domain: domain);
            return result;
        }
        /// <summary>
        /// Adds permission to file or a folder to all users
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder to which you want to add permission</param>
        /// <param name="role">Role which you want to add to a specific user for a file or folder</param>
        ///<returns>PGDriveResult with created permission in responseBody</returns>
        public PGDriveResult<Permission> AddPermissionAnyone(string fileOrFolderId, Roles role)
        {
            var result = CreateFilePermission(fileOrFolderId, Types.anyone, role);
            return result;
        }
    }
}

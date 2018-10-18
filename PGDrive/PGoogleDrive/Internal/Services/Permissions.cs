using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using PGoogleDrive.Internal.Models;
using PGoogleDrive.Internal.Services.PermissionsInternal;
using PGoogleDrive.Internal.Services.PermissionsInternal.Models;
using System.Collections.Generic;
using System.Linq;

namespace PGoogleDrive.Internal.Services
{
    public class Permissions : ServiceModule
    {
        static IDictionary<int, string> PermissionRoles = new Dictionary<int, string>(6);
        static IDictionary<int, string> PermissionTypes = new Dictionary<int, string>(4);

        public enum Types
        {
            user = 1,
            group,
            domain,
            anyone
        }
        public enum Roles
        {
            owner = 1,
            organizer,
            fileOrganizer,
            writer,
            commenter,
            reader
        }
        
        /// <summary>
        /// Sub-module which contains all methods to add permissions
        /// </summary>
        public CreatePermissionsModule AddPermissions {  get;  private set; }
        /// <summary>
        /// Sub-module which contains all methods to delete permissions
        /// </summary>
        public DeletePermissionsModule DeletePermissions { get; private set; }
        /// <summary>
        /// Initializes Permissions module with drive service
        /// </summary>
        /// <param name="driveService"></param>
        public Permissions(DriveService driveService) : base(driveService)
        {          
           
            AddPermissions = new CreatePermissions(this.driveService);
            DeletePermissions = new DeletePermissions(this.driveService);
            if (PermissionTypes.Count == 0)
            {
                InitializeTypes();
            }
            if (PermissionRoles.Count == 0)
            {
                InitializeRoles();
            }
        }

        void InitializeTypes()
        {
            PermissionTypes.Add(1, "user");
            PermissionTypes.Add(2, "group");
            PermissionTypes.Add(3, "domain");
            PermissionTypes.Add(4, "anyone");
        }

        void InitializeRoles()
        {
            PermissionRoles.Add(1, "owner");
            PermissionRoles.Add(2, "organizer");
            PermissionRoles.Add(3, "fileOrganizer");
            PermissionRoles.Add(4, "writer");
            PermissionRoles.Add(5, "commenter");
            PermissionRoles.Add(6, "reader");
        }


        internal static Permission CreatePermission(int type, int role, string domain=null, string email=null)
        {
            Permission permission = new Permission();
            permission.Type = PermissionTypes.Where(p => p.Key.Equals(type)).SingleOrDefault().Value;
            permission.Role = PermissionRoles.Where(p => p.Key.Equals(role)).SingleOrDefault().Value;
            if (permission.Role == null || permission.Type == null) { return null; }
            if(domain!=null) { permission.Domain = domain; }
            if(email!=null) { permission.EmailAddress = email; }
            return permission;
        }
        internal static PGDriveResult<IEnumerable<Permission>> GetFilePermissions(string fileId, DriveService service)
        {
            PGDriveResult<IEnumerable<Permission>> driveResult = new PGDriveResult<IEnumerable<Permission>>();
            try
            {
                var result = service.Permissions.List(fileId);
                result.Fields = "permissions(id, emailAddress, domain, type, role, displayName, kind)";
                var Request = result.Execute();
                driveResult.SetResponseBody(Request.Permissions);
                return driveResult;
            }
            catch (GoogleApiException exception)
            {
                driveResult.InitializeError(exception);
                return driveResult;
            }
        }
        /// <summary>
        /// Gets all permissions of file or folder
        /// </summary>
        /// <param name="fileOrFolderId">Id of file or folder to get permissions</param>
        /// <returns>PGDriveResult with collection of permissions in responseBody</returns>
        public PGDriveResult<IEnumerable<Permission>> GetFilePermissions(string fileOrFolderId)
        {
            var result = GetFilePermissions(fileOrFolderId, driveService);
            return result;
        }
    }
}

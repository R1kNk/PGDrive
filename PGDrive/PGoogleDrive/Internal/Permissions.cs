using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGoogleDrive.Internal
{
   internal static class Permissions
    {
        

        static Permissions()
        {
            PermissionRoles.Add(1, "owner");
            PermissionRoles.Add(2, "organizer");
            PermissionRoles.Add(3, "fileOrganizer");
            PermissionRoles.Add(4, "writer");
            PermissionRoles.Add(5, "commenter");
            PermissionRoles.Add(6, "reader");

            PermissionTypes.Add(1, "user");
            PermissionTypes.Add(2, "group");
            PermissionTypes.Add(3, "domain");
            PermissionTypes.Add(4, "anyone");

        }
        static IDictionary<int, string> PermissionRoles = new Dictionary<int, string>(6);
        static IDictionary<int, string> PermissionTypes = new Dictionary<int, string>(4);


        public static Permission CreatePermission(int type, int role, string domain=null, string email=null)
        {
            Permission permission = new Permission();
            permission.Type = PermissionTypes.Where(p => p.Key.Equals(type)).SingleOrDefault().Value;
            permission.Role = PermissionRoles.Where(p => p.Key.Equals(role)).SingleOrDefault().Value;
            if (permission.Role == null || permission.Type == null) { return null; }
            if(domain!=null) { permission.Domain = domain; }
            if(email!=null) { permission.EmailAddress = email; }
            return permission;
        }
    }
}

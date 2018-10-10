using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Drive.v3;

namespace PGoogleDrive
{
    public class PGDrive
    {
        public string ApplicationName { get; private set; }

        string clientSecretPath; 

        UserCredential credential;

        DriveService driveService;

        enum PermissionTypes
        {
            user = 1,
            group,
            domain,
            anyone
        }
        public enum PermissionRoles
        {
            owner = 1,
            organizer,
            fileOrganizer,
            writer,
            commenter,
            reader
        }

        public PGDrive(string pGDriveAttributeName)
        {
            //var config = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug","")+"/Web.config");
            //var targetNode = config.Root.Element("configSections");
            //XElement xElement = new XElement("section", new XAttribute("name", "pGDriveConfigs"), new XAttribute("type", "PGoogleDrive.PGDriveConfigsSection"));
            //targetNode.Add(xElement);
            //config.Save(AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "") + "/Web.config");
            
        }

        private bool SetFilePermission(string fileId, PermissionTypes type, PermissionRoles role, string email=null, string domain = null)
        {
            Permission newPermission = Internal.Permissions.CreatePermission((int)type, (int)role);
            if (email != null) newPermission.EmailAddress = email;
            else if (domain != null) newPermission.Domain = domain;
            var result = Internal.InternalMethods.SetFilePermission(fileId, driveService, newPermission);
            if (result == null) { return false; }
            return true;
        }

        private bool DeleteFilePermission(string fileId, PermissionTypes type, PermissionRoles role)
        {
            Permission newPermission = Internal.Permissions.CreatePermission((int)type, (int)role);
            var result = Internal.InternalMethods.DeleteFilePermission(fileId, driveService, newPermission);
            return result;
        }

        public bool SetFilePermissionUser(string fileId, PermissionRoles role, string userEmail)
        {
            var result = SetFilePermission(fileId, PermissionTypes.user, role, email: userEmail);
            return result;
        }

        public bool SetFilePermissionGroup(string fileId, PermissionRoles role, string groupEmail)
        {
            var result = SetFilePermission(fileId, PermissionTypes.group, role, email: groupEmail);
            return result;
        }

        public bool SetFilePermissionDomain(string fileId, PermissionRoles role, string domain)
        {
            var result = SetFilePermission(fileId, PermissionTypes.domain, role, domain: domain);
            return result;
        }

        public bool SetFilePermissionAnyone( string fileId, PermissionRoles role)
        {
            var result = SetFilePermission(fileId, PermissionTypes.anyone, role);
            return result;
        }

        public bool DeleteFilePermissionUser(string fileId, PermissionRoles role)
        {
            var result = DeleteFilePermission(fileId, PermissionTypes.user, role);
            return result;
        }

        public bool DeleteFilePermissionGroup(string fileId, PermissionRoles role)
        {
            var result = DeleteFilePermission(fileId, PermissionTypes.group, role);
            return result;
        }

        public bool DeleteFilePermissionDomain(string fileId, PermissionRoles role)
        {
            var result = DeleteFilePermission(fileId, PermissionTypes.domain, role);
            return result;
        }

        public bool DeleteFilePermissionAnyone(string fileId, PermissionRoles role)
        {
            var result = DeleteFilePermission(fileId, PermissionTypes.anyone, role);
            return result;
        }


    }
}

using System.Configuration;

namespace PGoogleDrive.Internal.Models
{

    public class PGDriveConfig
    {
        public static PGDriveConfigSection _Config = ConfigurationManager.GetSection("PGDrive") as PGDriveConfigSection;

        public static ApiKeyPGDriveElementCollection GetApiKeysDrives()
        {
            return _Config.ApiKeyDrives;
        }
        public static OAuthPGDriveElementCollection GetOAuthDrives()
        {
            return _Config.OAuthDrives;
        }

        public static OAuthGDriveElement GetOAuthElement(string Name)
        {
            foreach(OAuthGDriveElement element in GetOAuthDrives())
            {
                if (element.Name == Name) return element;
            }
            return null;
        }
        public static ApiKeyGDriveElement GetApiKeyElement(string Name)
        {
            foreach (ApiKeyGDriveElement element in GetApiKeysDrives())
            {
                if (element.Name == Name) return element;
            }
            return null;
        }
    }

    public class PGDriveConfigSection : ConfigurationSection
    {
        //Decorate the property with the tag for your collection.
        [ConfigurationProperty("OAuthDrives")]
        public OAuthPGDriveElementCollection OAuthDrives
        {
            get { return (OAuthPGDriveElementCollection)this["OAuthDrives"]; }
        }

        [ConfigurationProperty("ApiKeyDrives")]
        public ApiKeyPGDriveElementCollection ApiKeyDrives
        {
            get { return (ApiKeyPGDriveElementCollection)this["ApiKeyDrives"]; }
        }
    }

    [ConfigurationCollection(typeof(OAuthGDriveElement), AddItemName ="drive")]
    public class OAuthPGDriveElementCollection : ConfigurationElementCollection
    {
        public OAuthGDriveElement this[int index]
        {
            get { return (OAuthGDriveElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new OAuthGDriveElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((OAuthGDriveElement)element).Name;
        }
    }

    public class OAuthGDriveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("applicationName", IsRequired = true)]
        public string ApplicationName
        {
            get { return (string)this["applicationName"]; }
            set { this["applicationName"] = value; }
        }
        [ConfigurationProperty("clientSecretPath", IsRequired = true)]
        public string ClientSecretPath
        {
            get { return (string)this["clientSecretPath"]; }
            set { this["clientSecretPath"] = value; }
        }
        [ConfigurationProperty("default", IsRequired = false, DefaultValue = true)]
        public bool Default
        {
            get { return (bool)this["default"]; }
            set { this["default"] = value; }
        }
    }

    [ConfigurationCollection(typeof(OAuthGDriveElement), AddItemName ="drive")]
    public class ApiKeyPGDriveElementCollection : ConfigurationElementCollection
    {
        public ApiKeyGDriveElement this[int index]
        {
            get { return (ApiKeyGDriveElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new ApiKeyGDriveElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApiKeyGDriveElement)element).Name;
        }
    }
 
    public class ApiKeyGDriveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
        [ConfigurationProperty("applicationName", IsRequired = true)]
        public string ApplicationName
        {
            get { return (string)this["applicationName"]; }
            set { this["applicationName"] = value; }
        }
        [ConfigurationProperty("apiKey", IsRequired = true)]
        public string ApiKey
        {
            get { return (string)this["apiKey"]; }
            set { this["apiKey"] = value; }
        }
        [ConfigurationProperty("default", IsRequired = false, DefaultValue = true)]
        public bool Default
        {
            get { return (bool)this["default"]; }
            set { this["default"] = value; }
        }
    }

}


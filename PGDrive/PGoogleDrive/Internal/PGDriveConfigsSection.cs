using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PGoogleDrive
{

    //This class reads the defined config section (if available) and stores it locally in the static _Config variable.
    //This config data is available by calling MedGroups.GetMedGroups().
    internal class PGDrives
    {
        public static PGDriveConfigsSection _Config = ConfigurationManager.GetSection("pGDriveConfigs") as PGDriveConfigsSection;
        public static PGDriveElementCollection GetMedGroups()
        {
            return _Config.GDrives;
        }
    }
    //Extend the ConfigurationSection class.  Your class name should match your section name and be postfixed with "Section".
    internal class PGDriveConfigsSection : ConfigurationSection
    {
        //Decorate the property with the tag for your collection.
        [ConfigurationProperty("pgDrives")]
        public PGDriveElementCollection GDrives
        {
            get { return (PGDriveElementCollection)this["pgDrives"]; }
        }
    }
    //Extend the ConfigurationElementCollection class.
    //Decorate the class with the class that represents a single element in the collection.
    [ConfigurationCollection(typeof(GDriveElement))]
    internal class PGDriveElementCollection : ConfigurationElementCollection
    {
        public GDriveElement this[int index]
        {
            get { return (GDriveElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new GDriveElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((GDriveElement)element).Name;
        }
    }
    //Extend the ConfigurationElement class.  This class represents a single element in the collection.
    //Create a property for each xml attribute in your element.
    //Decorate each property with the ConfigurationProperty decorator.  See MSDN for all available options.
    internal class GDriveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
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
        
}


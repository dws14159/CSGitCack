using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CSGitCack
{
    public class Config
    {
        public string FileVersion = "DrillRigDemo Config V1.0";
        [XmlIgnore] public bool LoadOK;

        public List<ZoneInfo> ZonesInfo { get; set; } = new List<ZoneInfo>();
        //public List<TagFriendlyName> TagFriendlyNames { get; set; } = new List<TagFriendlyName>();
        //public List<string> TagsSeen { get; set; } = new List<string>();
        //public string BackgroundImage { get; set; }
        //public List<string> OverlayFiles { get; set; } = new List<string>();

        private string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DrillRigDemo.xml");
        }
    }
}

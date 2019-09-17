using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSGitCack
{
    public class Transform
    {
        [XmlElement(Order = 1)] public string DestTables;
        [XmlElement(Order = 2)] public string QueryText;

        [XmlArray(Order = 3)] [XmlArrayItem(ElementName = "Task")]
        public List<string> TaskList;
    }
}

using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSGitCack
{
    public class DataSource
    {
        [XmlElement(Order = 1)] public string ConnectionString;
        [XmlElement(Order = 2)] public List<Transform> Transformation;
    }
}

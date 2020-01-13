using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Utilities
{
    public class DbGroupConfiguration : BaseConfig<DbGroupConfiguration>
    {

        public static DbGroupConfiguration Instance = new DbGroupConfiguration();


        public override string GetPathName()
        {
            return "DbGroup.config";
        }

        [XmlElement]
        public DbGroup DbGroup { get; set; }
    }
    public class DbGroup
    {
        [XmlElement(ElementName = "DbNode")]
        public List<DbNode> DbNodes { get; set; }
        [XmlAttribute]
        public int Interval { get; set; }   //侦测的时间间隔
    }
    public class DbNode 
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string ConnectionString { get; set; }
        public DbNodeState State { get; set; }
        [XmlAttribute]
        public DbNodeType NodeType { get; set; }
        [XmlAttribute]
        public string ProviderName { get; set; }     //数据提供驱动
        public DateTime LastModifyTime { get; set; } //最后一次侦测时间

    }

    public class DbNodeType
    {
    }

    public class DbNodeState
    {
    }
}

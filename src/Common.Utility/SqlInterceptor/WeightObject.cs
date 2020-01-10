using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Common.Utilities
{
    public class WeightObject
    {
        /// <summary>
        /// 权重
        /// </summary>
        [XmlAttribute]
        public int Weight { set; get; }
    }
}

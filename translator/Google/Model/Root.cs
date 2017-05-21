using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace translator.Google.Model
{
    [DataContract]
    public class Root
    {
        /// <summary>
        /// Represents multiple files returned in a JSON request
        /// </summary>
        [DataMember(Name = "data")]
        public List<Data> data { get; set; }
    }
}

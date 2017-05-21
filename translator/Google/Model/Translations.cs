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

namespace translator.Google.Model
{
    [DataContract]
    public class Translations
    {
        [DataMember(Name = "translatedText")]
        public string translatedText
        {
            get;
            set;
        }


        //[DataMember(Name = "detectedSourceLanguage")]
        //public string DetectedSourceLanguage
        //{
        //    get;
        //    set;
        //}
    }
}

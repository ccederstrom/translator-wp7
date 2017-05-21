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

namespace translator
{
    public class Languages
    {
        public Languages() { }
        public Languages(Languages languages)
        {
            this.FlagImagePath = languages.FlagImagePath;
            this.Full = languages.Full;
            this.Short = languages.Short;
            this.Speech = languages.Speech;
        }

        public string Full
        {
            get;
            set;
        }

        public string Short
        {
            get;
            set;
        }

        public Boolean Speech
        {
            get;
            set;
        }

        public string FlagImagePath
        {
            get;
            set;
        }


        public bool isBing { get; set; }

        public bool isBothAvailable { get; set; }
    }
}

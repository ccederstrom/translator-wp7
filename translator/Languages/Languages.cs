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
using System.Collections.Generic;

namespace translator.Languages
{
    public class Languages
    {
        List<Languages> mTo_Languages;
        List<Languages> mFrom_Languages;


        public List<Languages> To
        {
            get {
                return mTo_Languages;
            }
            set
            {
                if (mTo_Languages != value)
                {
                    mTo_Languages = value;
                }
            }
        }

        public List<Languages> From
        {
            get
            {
                return mFrom_Languages;
            }
            set
            {
                if (mFrom_Languages != value)
                {
                    mFrom_Languages = value;
                }
            }
        }

        public Languages()
        {
            Initialize();

        }


        public void Initialize()
        {
            mTo_Languages = new List<Languages>();
            mFrom_Languages = new List<Languages>();

            #region Initialize Languages
            
            #region Languages for Text translation
            // Google languages http://code.google.com/apis/language/translate/v2/using_rest.html
            // languages for text translation
            //      - <ArrayOfstring xmlns="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            mFrom_Languages.Add(new Languages() { Full = "Afrikaans *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Afrikaans.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Albanian *", Short = "sq", Speech = false, FlagImagePath = "/Images/Flags/200px-Albanian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Arabic *", Short = "ar", Speech = false, FlagImagePath = "/Images/Flags/200px-Arabic.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Azerbaijani *", Short = "az", Speech = false, FlagImagePath = "/Images/Flags/200px-Azerbaijani.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Basque *", Short = "eu", Speech = false, FlagImagePath = "/Images/Flags/200px-Basque.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Bengali *", Short = "bn", Speech = false, FlagImagePath = "/Images/Flags/200px-Bengali.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Belarusian *", Short = "be", Speech = false, FlagImagePath = "/Images/Flags/200px-Belarusian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Bulgarian *", Short = "bg", Speech = false, FlagImagePath = "/Images/Flags/200px-Bulgarian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Catalan", Short = "ca", Speech = true, FlagImagePath = "/Images/Flags/200px-Catalan.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Chinese Simpified", Short = "zh-CHS", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mFrom_Languages.Add(new Languages() { Full = "Chinese Traditional", Short = "zh-CHT", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese_Traditional.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mFrom_Languages.Add(new Languages() { Full = "Croatian *", Short = "hr", Speech = false, FlagImagePath = "/Images/Flags/200px-Croatian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Czech *", Short = "cs", Speech = false, FlagImagePath = "/Images/Flags/200px-Czech.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Danish", Short = "da", Speech = true, FlagImagePath = "/Images/Flags/200px-Danish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Dutch", Short = "nl", Speech = true, FlagImagePath = "/Images/Flags/200px-Dutch.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "English", Short = "en", Speech = true, FlagImagePath = "/Images/Flags/200px-English.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Esperanto *", Short = "eo", Speech = false, FlagImagePath = "/Images/Flags/200px-Esperanto.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Estonian *", Short = "et", Speech = false, FlagImagePath = "/Images/Flags/200px-Estonian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Filipino *", Short = "tl", Speech = false, FlagImagePath = "/Images/Flags/200px-Filipino.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Finnish", Short = "fi", Speech = true, FlagImagePath = "/Images/Flags/200px-Finnish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "French", Short = "fr", Speech = true, FlagImagePath = "/Images/Flags/200px-French.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Galician *", Short = "gl", Speech = false, FlagImagePath = "/Images/Flags/200px-Galician.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Georgian *", Short = "ka", Speech = false, FlagImagePath = "/Images/Flags/200px-Georgian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "German", Short = "de", Speech = true, FlagImagePath = "/Images/Flags/200px-German.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Greek *", Short = "el", Speech = false, FlagImagePath = "/Images/Flags/200px-Greek.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Gujarati *", Short = "gu", Speech = false, FlagImagePath = "/Images/Flags/200px-Gujarati.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Haitian Creole *", Short = "ht", Speech = false, FlagImagePath = "/Images/Flags/200px-Haitian_Creole.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Hebrew *", Short = "he", Speech = false, FlagImagePath = "/Images/Flags/200px-Hebrew.png", isBing = true, isBothAvailable = false }); // google code different
            mFrom_Languages.Add(new Languages() { Full = "Hindi *", Short = "hi", Speech = false, FlagImagePath = "/Images/Flags/200px-Hindi.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Hungarian *", Short = "hu", Speech = false, FlagImagePath = "/Images/Flags/200px-Hungarian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Icelandic *", Short = "is", Speech = false, FlagImagePath = "/Images/Flags/200px-Icelandic.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Indonesian *", Short = "id", Speech = false, FlagImagePath = "/Images/Flags/200px-Indonesian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Irish *", Short = "ga", Speech = false, FlagImagePath = "/Images/Flags/200px-Irish.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Italian", Short = "it", Speech = true, FlagImagePath = "/Images/Flags/200px-Italian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Japanese", Short = "ja", Speech = true, FlagImagePath = "/Images/Flags/200px-Japanese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Kannada *", Short = "kn", Speech = false, FlagImagePath = "/Images/Flags/200px-Kannada.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Korean", Short = "ko", Speech = true, FlagImagePath = "/Images/Flags/200px-Korean.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Latin *", Short = "la", Speech = false, FlagImagePath = "/Images/Flags/200px-Latin.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Latvian *", Short = "lv", Speech = false, FlagImagePath = "/Images/Flags/200px-Latvian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Lithuanian", Short = "lt", Speech = true, FlagImagePath = "/Images/Flags/200px-Lithuanian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Macedonian *", Short = "mk", Speech = false, FlagImagePath = "/Images/Flags/200px-Macedonian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Malay *", Short = "ms", Speech = false, FlagImagePath = "/Images/Flags/200px-Malay.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Maltese *", Short = "mt", Speech = false, FlagImagePath = "/Images/Flags/200px-Maltese.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Norwegian", Short = "no", Speech = true, FlagImagePath = "/Images/Flags/200px-Norwegian.png", isBing = true, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Persian *", Short = "fa", Speech = false, FlagImagePath = "/Images/Flags/200px-Persian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Polish", Short = "pl", Speech = true, FlagImagePath = "/Images/Flags/200px-Polish.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Portuguese", Short = "pt", Speech = true, FlagImagePath = "/Images/Flags/200px-Portuguese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Romanian *", Short = "ro", Speech = false, FlagImagePath = "/Images/Flags/200px-Romanian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Russian", Short = "ru", Speech = true, FlagImagePath = "/Images/Flags/200px-Russian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Serbian *", Short = "sr", Speech = false, FlagImagePath = "/Images/Flags/200px-Serbian.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Slovak *", Short = "sk", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovak.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Slovenian *", Short = "sl", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovenian.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Spanish", Short = "es", Speech = true, FlagImagePath = "/Images/Flags/200px-Spanish.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Swahili *", Short = "sw", Speech = false, FlagImagePath = "/Images/Flags/200px-Swahili.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Swedish", Short = "sv", Speech = true, FlagImagePath = "/Images/Flags/200px-Swedish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Tamil *", Short = "ta", Speech = false, FlagImagePath = "/Images/Flags/200px-Tamil.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Telugu *", Short = "te", Speech = false, FlagImagePath = "/Images/Flags/200px-Telugu.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Thai *", Short = "th", Speech = false, FlagImagePath = "/Images/Flags/200px-Thai.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Turkish *", Short = "tr", Speech = false, FlagImagePath = "/Images/Flags/200px-Turkish.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Ukrainian *", Short = "uk", Speech = false, FlagImagePath = "/Images/Flags/200px-Ukrainian.png", isBothAvailable = true, isBing = true });
            mFrom_Languages.Add(new Languages() { Full = "Urdu *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Urdu.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Vietnamese *", Short = "vi", Speech = false, FlagImagePath = "/Images/Flags/200px-Vietnamese.png", isBing = true, isBothAvailable = true });
            mFrom_Languages.Add(new Languages() { Full = "Welsh *", Short = "cy", Speech = false, FlagImagePath = "/Images/Flags/200px-Welsh.png", isBing = false, isBothAvailable = false });
            mFrom_Languages.Add(new Languages() { Full = "Yiddish *", Short = "yi", Speech = false, FlagImagePath = "/Images/Flags/200px-Yiddish.png", isBing = false, isBothAvailable = false });

            //</ArrayOfstring>

            #endregion

            
            #region Languages for Text translation
            // languages for text translation
            //      - <ArrayOfstring xmlns="http://schemas.microsoft.com/2003/10/Serialization/Arrays" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
            mTo_Languages.Add(new Languages() { Full = "Afrikaans *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Afrikaans.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Albanian *", Short = "sq", Speech = false, FlagImagePath = "/Images/Flags/200px-Albanian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Arabic *", Short = "ar", Speech = false, FlagImagePath = "/Images/Flags/200px-Arabic.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Azerbaijani *", Short = "az", Speech = false, FlagImagePath = "/Images/Flags/200px-Azerbaijani.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Basque *", Short = "eu", Speech = false, FlagImagePath = "/Images/Flags/200px-Basque.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Bengali *", Short = "bn", Speech = false, FlagImagePath = "/Images/Flags/200px-Bengali.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Belarusian *", Short = "be", Speech = false, FlagImagePath = "/Images/Flags/200px-Belarusian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Bulgarian *", Short = "bg", Speech = false, FlagImagePath = "/Images/Flags/200px-Bulgarian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Catalan", Short = "ca", Speech = true, FlagImagePath = "/Images/Flags/200px-Catalan.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Chinese Simpified", Short = "zh-CHS", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mTo_Languages.Add(new Languages() { Full = "Chinese Traditional", Short = "zh-CHT", Speech = true, FlagImagePath = "/Images/Flags/200px-Chinese_Traditional.png", isBing = true, isBothAvailable = false }); // Google's code is different
            mTo_Languages.Add(new Languages() { Full = "Croatian *", Short = "hr", Speech = false, FlagImagePath = "/Images/Flags/200px-Croatian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Czech *", Short = "cs", Speech = false, FlagImagePath = "/Images/Flags/200px-Czech.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Danish", Short = "da", Speech = true, FlagImagePath = "/Images/Flags/200px-Danish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Dutch", Short = "nl", Speech = true, FlagImagePath = "/Images/Flags/200px-Dutch.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "English", Short = "en", Speech = true, FlagImagePath = "/Images/Flags/200px-English.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Esperanto *", Short = "eo", Speech = false, FlagImagePath = "/Images/Flags/200px-Esperanto.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Estonian *", Short = "et", Speech = false, FlagImagePath = "/Images/Flags/200px-Estonian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Filipino *", Short = "tl", Speech = false, FlagImagePath = "/Images/Flags/200px-Filipino.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Finnish", Short = "fi", Speech = true, FlagImagePath = "/Images/Flags/200px-Finnish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "French", Short = "fr", Speech = true, FlagImagePath = "/Images/Flags/200px-French.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Galician *", Short = "gl", Speech = false, FlagImagePath = "/Images/Flags/200px-Galician.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Georgian *", Short = "ka", Speech = false, FlagImagePath = "/Images/Flags/200px-Georgian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "German", Short = "de", Speech = true, FlagImagePath = "/Images/Flags/200px-German.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Greek *", Short = "el", Speech = false, FlagImagePath = "/Images/Flags/200px-Greek.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Gujarati *", Short = "gu", Speech = false, FlagImagePath = "/Images/Flags/200px-Gujarati.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Haitian Creole *", Short = "ht", Speech = false, FlagImagePath = "/Images/Flags/200px-Haitian_Creole.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Hebrew *", Short = "he", Speech = false, FlagImagePath = "/Images/Flags/200px-Hebrew.png", isBing = true, isBothAvailable = false }); // google code different
            mTo_Languages.Add(new Languages() { Full = "Hindi *", Short = "hi", Speech = false, FlagImagePath = "/Images/Flags/200px-Hindi.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Hungarian *", Short = "hu", Speech = false, FlagImagePath = "/Images/Flags/200px-Hungarian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Icelandic *", Short = "is", Speech = false, FlagImagePath = "/Images/Flags/200px-Icelandic.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Indonesian *", Short = "id", Speech = false, FlagImagePath = "/Images/Flags/200px-Indonesian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Irish *", Short = "ga", Speech = false, FlagImagePath = "/Images/Flags/200px-Irish.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Italian", Short = "it", Speech = true, FlagImagePath = "/Images/Flags/200px-Italian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Japanese", Short = "ja", Speech = true, FlagImagePath = "/Images/Flags/200px-Japanese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Kannada *", Short = "kn", Speech = false, FlagImagePath = "/Images/Flags/200px-Kannada.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Korean", Short = "ko", Speech = true, FlagImagePath = "/Images/Flags/200px-Korean.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Latin *", Short = "la", Speech = false, FlagImagePath = "/Images/Flags/200px-Latin.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Latvian *", Short = "lv", Speech = false, FlagImagePath = "/Images/Flags/200px-Latvian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Lithuanian", Short = "lt", Speech = true, FlagImagePath = "/Images/Flags/200px-Lithuanian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Macedonian *", Short = "mk", Speech = false, FlagImagePath = "/Images/Flags/200px-Macedonian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Malay *", Short = "ms", Speech = false, FlagImagePath = "/Images/Flags/200px-Malay.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Maltese *", Short = "mt", Speech = false, FlagImagePath = "/Images/Flags/200px-Maltese.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Norwegian", Short = "no", Speech = true, FlagImagePath = "/Images/Flags/200px-Norwegian.png", isBing = true, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Persian *", Short = "fa", Speech = false, FlagImagePath = "/Images/Flags/200px-Persian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Polish", Short = "pl", Speech = true, FlagImagePath = "/Images/Flags/200px-Polish.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Portuguese", Short = "pt", Speech = true, FlagImagePath = "/Images/Flags/200px-Portuguese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Romanian *", Short = "ro", Speech = false, FlagImagePath = "/Images/Flags/200px-Romanian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Russian", Short = "ru", Speech = true, FlagImagePath = "/Images/Flags/200px-Russian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Serbian *", Short = "sr", Speech = false, FlagImagePath = "/Images/Flags/200px-Serbian.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Slovak *", Short = "sk", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovak.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Slovenian *", Short = "sl", Speech = false, FlagImagePath = "/Images/Flags/200px-Slovenian.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Spanish", Short = "es", Speech = true, FlagImagePath = "/Images/Flags/200px-Spanish.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Swahili *", Short = "sw", Speech = false, FlagImagePath = "/Images/Flags/200px-Swahili.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Swedish", Short = "sv", Speech = true, FlagImagePath = "/Images/Flags/200px-Swedish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Tamil *", Short = "ta", Speech = false, FlagImagePath = "/Images/Flags/200px-Tamil.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Telugu *", Short = "te", Speech = false, FlagImagePath = "/Images/Flags/200px-Telugu.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Thai *", Short = "th", Speech = false, FlagImagePath = "/Images/Flags/200px-Thai.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Turkish *", Short = "tr", Speech = false, FlagImagePath = "/Images/Flags/200px-Turkish.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Ukrainian *", Short = "uk", Speech = false, FlagImagePath = "/Images/Flags/200px-Ukrainian.png", isBothAvailable = true, isBing = true });
            mTo_Languages.Add(new Languages() { Full = "Urdu *", Short = "af", Speech = false, FlagImagePath = "/Images/Flags/200px-Urdu.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Vietnamese *", Short = "vi", Speech = false, FlagImagePath = "/Images/Flags/200px-Vietnamese.png", isBing = true, isBothAvailable = true });
            mTo_Languages.Add(new Languages() { Full = "Welsh *", Short = "cy", Speech = false, FlagImagePath = "/Images/Flags/200px-Welsh.png", isBing = false, isBothAvailable = false });
            mTo_Languages.Add(new Languages() { Full = "Yiddish *", Short = "yi", Speech = false, FlagImagePath = "/Images/Flags/200px-Yiddish.png", isBing = false, isBothAvailable = false });
            //</ArrayOfstring>

            #endregion
            #endregion

        }



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

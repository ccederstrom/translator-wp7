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
using Microsoft;
using System.Data.Services.Client;
using System.Diagnostics;

namespace translator.AzureDataMarket
{
    public class TranslatorData
    {
        // http://msdn.microsoft.com/en-us/library/gg193417
        // HTTP Basic Authentication
        // metadata https://api.datamarket.azure.com/Data.ashx/Bing/MicrosoftTranslator/$metadata
        readonly string CUSTOMER_ID = "f182d728-d0c4-44e4-bc5e-417179b90d1e";
        readonly string PRIMARY_ACCOUNT_KEY = "VLTB5s1HHQF4YTRR6MfXbrfQwBnPKcRSJya9qsceJAI=";
        readonly string SERVICE_ROOT_URL = "https://api.datamarket.azure.com/Bing/MicrosoftTranslator/ ";


        public event EventHandler TranslationChanged;
        public string translate;
        public string TranslatedText
        {
            get { return this.translate; }
            set
            {
                this.translate = value;
                if (this.TranslationChanged != null)
                    this.TranslationChanged(this, new EventArgs());
            }
        }


        Uri serviceUri;
        TranslatorContainer context;

        /// <summary>
        /// Initalize credentails
        /// </summary>
        public TranslatorData()
        {
            Initalize();
            //var query = context.Translate("Mr. Splashy Pants", "nl", "en");
            //query.BeginExecute(OnTranslateQueryComplete, query);
        }

    //    TextBox translatedTextBox;

        private void Initalize()
        {
            serviceUri = new Uri(SERVICE_ROOT_URL);
            context = new TranslatorContainer(serviceUri);
            context.Credentials = new NetworkCredential(CUSTOMER_ID, PRIMARY_ACCOUNT_KEY);
        }

        /// <summary>
        /// Facade for text translation
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void Translate(string text, string to, string from)
        {
            Initalize();
            try
            {
                //if (uiTranslatedTextBox != null)
                //    translatedTextBox = uiTranslatedTextBox;
                // Mr. Splashy pants was here!
                var query = context.Translate(text, to, from);
                query.BeginExecute(OnTranslateQueryComplete, query);
            }
            catch (DataServiceQueryException ex)
            {
                Debug.WriteLine("An error occurred during query execution." + ex.ToString());
                throw new Exception("An error occurred during query execution.", ex);
            }
        }

        /// <summary>
        /// Async request for text translation
        /// </summary>
        /// <param name="result"></param>
        private void OnTranslateQueryComplete(IAsyncResult result)
        {
            //     var query = (DataServiceQuery<Microsoft.Language>)result.AsyncState;
            var query = (DataServiceQuery<Microsoft.Translation>)result.AsyncState;
            var text = query.EndExecute(result);
            string translation = "";
            foreach (Translation t in text)
            {
                translation += t.Text;// +"\n";
            }
            Debug.WriteLine("Bing Translation:" + translation);
            TranslatedText = translation; // trigger eventhandler
        }

        /// <summary>
        /// Returna  list of language codes
        /// </summary>
        public void GetLanguagesForTranslation()
        {
            Initalize();
            try
            {
                var query = context.GetLanguagesForTranslation();
                query.BeginExecute(OnGetLanguageQueryComplete, query);
            }
            catch (DataServiceQueryException ex)
            {
                Debug.WriteLine("An error occurred during query execution." + ex.ToString());
                throw new Exception("An error occurred during query execution.", ex);
            }
        }

        /// <summary>
        /// Async request for language list
        /// </summary>
        /// <param name="result"></param>
        private void OnGetLanguageQueryComplete(IAsyncResult result)
        {
            var query = (DataServiceQuery<Microsoft.Language>)result.AsyncState;
            var enumerableLanguages = query.EndExecute(result);
            string langstring = "";
            foreach (Microsoft.Language lang in enumerableLanguages)
            {
                langstring += lang.Code + "\n";
            }
            Debug.WriteLine("Bing language codes:" + langstring);
        }
    }
}

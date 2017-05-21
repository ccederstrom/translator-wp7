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
using translator.Database.Model;
using System.Diagnostics;

namespace translator.Database
{
    public static class DatabaseController
    {
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/hh286405(v=VS.92).aspx
        /// </summary>
        public static void DatabaseDeleteHistory(TranslatorHistory historyForDelete)
        {
            // Get a handle for the to-do item bound to the button.
            //TranslatorHistory historyForDelete = data.DataContext as TranslatorHistory;
            App.ViewModel.DeleteTranslatorHistory(historyForDelete);
            App.ViewModel.SaveChangesToDB();
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/hh286405(v=VS.92).aspx
        /// </summary>
        public static void DatabaseAddHistory(int fromLanguageIndex, string fromLanguage, string fromLanguageCode, string fromText, int toLanguageIndex, string toLanguage, string toLanguageCode, string toText)
        {
            Debug.WriteLine("Calling DatabaseAddHistory");
            // Create a new to-do item.
            TranslatorHistory newTranslatorHistory = new TranslatorHistory
            {
                FromLanguageIndex = fromLanguageIndex,
                FromLanguage = fromLanguage,
                FromLanguageCode = fromLanguageCode,
                FromText = fromText,
                ToLanguageIndex = toLanguageIndex,
                ToLanguage = toLanguage,
                ToLanguageCode = toLanguageCode,
                ToText = toText,
                IsFavorite = false
            };

            // Add the item to the ViewModel.
            App.ViewModel.AddTranslatorHistory(newTranslatorHistory);
            App.ViewModel.SaveChangesToDB();
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/bb907191.aspx
        /// </summary>
        /// <param name="translatorHistory"></param>
        public static void DatabaseUpdateHistoryFavorite(TranslatorHistory translatorHistory)
        {
            App.ViewModel.UpdateTranslatorHistoryFavorite(translatorHistory);
            App.ViewModel.SaveChangesToDB();
        }

    }
}

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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;


using translator.Database.Model;
// http://msdn.microsoft.com/en-us/library/hh286405(v=VS.92).aspx

namespace translator.Database.ViewModel
{
    public class TranslatorViewModel : INotifyPropertyChanged, IViewModel
    {
        // LINQ to SQL data context for the local database.
        private TranslatorDataContext translatorDB;

        // Class constructor, create the data context object.
        public TranslatorViewModel(string translatorDBConnectionString)
        {
            translatorDB = new TranslatorDataContext(translatorDBConnectionString);
        }

        // All history items.
        private ObservableCollection<TranslatorHistory> _allTranslatorHistory;
        public ObservableCollection<TranslatorHistory> AllTranslatorHistory
        {
            get { return _allTranslatorHistory; }
            set
            {
                _allTranslatorHistory = value;
                NotifyPropertyChanged("AllTranslatorHistory");

            }
        }


        // favorite history items.
        private ObservableCollection<TranslatorHistory> _favoriteTranslatorHistory;
        public ObservableCollection<TranslatorHistory> FavoriteTranslatorHistory
        {
            get { return _favoriteTranslatorHistory; }
            set
            {
                _favoriteTranslatorHistory = value;
                NotifyPropertyChanged("FavoriteTranslatorHistory");
            }
        }

     



        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            translatorDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion




        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all translator history in the database.
            var translatorHistoryInDB = from TranslatorHistory history in translatorDB.History
                                select history;

            // Query the database and load all translator history.
            AllTranslatorHistory = new ObservableCollection<TranslatorHistory>(translatorHistoryInDB);


            // Specify the query for all translator history in the database.
            var favoriteHistoryInDB = from TranslatorHistory history in translatorDB.History where history.IsFavorite == true
                                        select history;

            // Query the database and load all translator history.
            FavoriteTranslatorHistory = new ObservableCollection<TranslatorHistory>(favoriteHistoryInDB);

        }


        // Add a translator history to the database and collections.
        public void AddTranslatorHistory(TranslatorHistory newTranslatorHistory)
        {
            // Add a to-do item to the data context.
            translatorDB.History.InsertOnSubmit(newTranslatorHistory);

            // Save changes to the database.
            translatorDB.SubmitChanges();

            AllTranslatorHistory.Add(newTranslatorHistory);

        }



        // Add a translator history to the database and collections.
        public void UpdateTranslatorHistoryFavorite(TranslatorHistory updateTranslatorHistory)
        {

            var updateHistoryInDB = (from TranslatorHistory history in translatorDB.History
                                      where history.HistoryId == updateTranslatorHistory.HistoryId
                                      select history).ToList()[0];

            // Add a to-do item to the data context.
            // toggle favorite 
            if (updateTranslatorHistory.IsFavorite == true)
            {
                updateHistoryInDB.IsFavorite = false;
                FavoriteTranslatorHistory.Remove(updateTranslatorHistory);
            }
            else
            {
                updateHistoryInDB.IsFavorite = true;
                FavoriteTranslatorHistory.Add(updateTranslatorHistory);
            }

            // Save changes to the database.
            translatorDB.SubmitChanges();
        }


        // Remove a translator history from the database and collections.
        public void DeleteTranslatorHistory(TranslatorHistory historyForDelete)
        {

            if (historyForDelete.IsFavorite == true)
            {
                // remove from both list boxes
                // Remove the to-do item from the "all" observable collection.
                AllTranslatorHistory.Remove(historyForDelete);
                FavoriteTranslatorHistory.Remove(historyForDelete);
            }
            else
            {
                // only remove from main listbox
                // Remove the to-do item from the "all" observable collection.
                AllTranslatorHistory.Remove(historyForDelete);
            }


            // Remove the to-do item from the data context.
            translatorDB.History.DeleteOnSubmit(historyForDelete);

            // Save changes to the database.
            translatorDB.SubmitChanges();
        }

    }
}

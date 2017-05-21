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
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

// http://msdn.microsoft.com/en-us/library/hh286405(v=VS.92).aspx

namespace translator.Database.Model
{
    [Table]
    public class TranslatorHistory : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _historyId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int HistoryId
        {
            get { return _historyId; }
            set
            {
                if (_historyId != value)
                {
                    NotifyPropertyChanging("HistoryId");
                    _historyId = value;
                    NotifyPropertyChanged("HistoryId");
                }
            }
        }


        // Define item name: private field, public property, and database column.
        private int _fromLanguageIndex;

        [Column]
        public int FromLanguageIndex
        {
            get { return _fromLanguageIndex; }
            set
            {
                if (_fromLanguageIndex != value)
                {
                    NotifyPropertyChanging("FromLanguageIndex");
                    _fromLanguageIndex = value;
                    NotifyPropertyChanged("FromLanguageIndex");
                }
            }
        }


        // Define item name: private field, public property, and database column.
        private string _fromLanguage;

        [Column]
        public string FromLanguage
        {
            get { return _fromLanguage; }
            set
            {
                if (_fromLanguage != value)
                {
                    NotifyPropertyChanging("FromLanguage");
                    _fromLanguage = value;
                    NotifyPropertyChanged("FromLanguage");
                }
            }
        }


        // Define item name: private field, public property, and database column.
        private string _fromLanguageCode;

        [Column]
        public string FromLanguageCode
        {
            get { return _fromLanguageCode; }
            set
            {
                if (_fromLanguageCode != value)
                {
                    NotifyPropertyChanging("FromLanguageCode");
                    _fromLanguageCode = value;
                    NotifyPropertyChanged("FromLanguageCode");
                }
            }
        }



        // Define completion value: private field, public property, and database column.
        private int _toLanguageIndex;

        [Column]
        public int ToLanguageIndex
        {
            get { return _toLanguageIndex; }
            set
            {
                if (_toLanguageIndex != value)
                {
                    NotifyPropertyChanging("ToLanguageIndex");
                    _toLanguageIndex = value;
                    NotifyPropertyChanged("ToLanguageIndex");
                }
            }
        }


        // Define completion value: private field, public property, and database column.
        private string _toLanguage;

        [Column]
        public string ToLanguage
        {
            get { return _toLanguage; }
            set
            {
                if (_toLanguage != value)
                {
                    NotifyPropertyChanging("ToLanguage");
                    _toLanguage = value;
                    NotifyPropertyChanged("ToLanguage");
                }
            }
        }


        // Define completion value: private field, public property, and database column.
        private string _toLanguageCode;

        [Column]
        public string ToLanguageCode
        {
            get { return _toLanguageCode; }
            set
            {
                if (_toLanguageCode != value)
                {
                    NotifyPropertyChanging("ToLanguageCode");
                    _toLanguageCode = value;
                    NotifyPropertyChanged("ToLanguageCode");
                }
            }
        }


        private string _fromText;

        [Column]
        public string FromText
        {
            get { return _fromText; }
            set
            {
                if (_fromText != value)
                {
                    NotifyPropertyChanging("FromText");
                    _fromText = value;
                    NotifyPropertyChanged("FromText");
                }
            }
        }


        private string _toText;

        [Column]
        public string ToText
        {
            get { return _toText; }
            set
            {
                if (_toText != value)
                {
                    NotifyPropertyChanging("ToText");
                    _toText = value;
                    NotifyPropertyChanged("ToText");
                }
            }
        }


        private bool _isFavorite;

        [Column]
        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                if (_isFavorite != value)
                {
                    NotifyPropertyChanging("IsFavorite");
                    _isFavorite = value;
                    NotifyPropertyChanged("IsFavorite");
                }
            }
        }



        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }

    public class TranslatorDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public TranslatorDataContext(string connectionString)
            : base(connectionString)
        { }



        // Specify a table for the history.
        public Table<TranslatorHistory> History;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace translator.Database.Model
{
    interface IViewModel
    {
        void SaveChangesToDB();
        void LoadCollectionsFromDatabase();
    }
}

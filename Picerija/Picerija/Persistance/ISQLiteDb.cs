using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Picerija.Persistance
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

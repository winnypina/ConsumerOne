using System;
using Akavache.Sqlite3;

namespace ConsumerOne.Mobile.iOS
{

    [Foundation.Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(SQLitePersistentBlobCache c)
        {
            c.Dispose();
            var c2 = new SQLitePersistentBlobCache("");
            c2.Dispose();
        }
    }
}

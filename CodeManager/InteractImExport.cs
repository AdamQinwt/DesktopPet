using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    public partial class FrmInteract
    {
        //import data entries and add to a table
        //name=[tableName,fName]
        private String importData(String[] name)
        {
            if (name.Length < 1) return "Insufficient Params. Expecting tableName[,fName[,format]].";
            String tname = name[0],fname,fmt;
            fmt = name.Length < 3 ? "txt" : name[2];
            fname = name.Length < 2 ? (tname+"."+fmt) : name[1];
            return db.importTable(tname,fname,fmt);
        }
        //export data entries to a file
        //name=[tableName,fName]
        private String exportData(String[] name)
        {
            if (name.Length < 1) return "Insufficient Params. Expecting tableName[,fName[,format]].";
            String tname = name[0], fname, fmt;
            fmt = name.Length < 3 ? "txt" : name[2];
            fname = name.Length < 2 ? (tname + "." + fmt) : name[1];
            return db.exportTable(tname, fname,fmt);
        }
    }
}

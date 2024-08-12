using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CodeManager
{
    class DBManager
    {
        SQLiteConnection sql;
        SQLiteCommand cmd;
        SQLiteDataReader reader;
        String tableName, descriptionStr;
        int numFields;
        String[] fields;
        public String[] FieldNames { get { return fields; } }
        public String[] requiredDataFields;
        public enum FieldType
        {
            id=0,
            int_,
            float_,
            text,
            longtext,
        }
        FieldType[] fieldTypes;
        Dictionary<String, FieldType> mapField2Type;
        static String[] SpecialSymbolsFrom, SpecialSymbolsTo;
        public string dataField;
        public const string defaultIdField = "_id";
        public const string defaultNameField = "name";
        //string DataField;   //default field for core data(to be copied)
        //string[] summaryFields; //default fields for list summary
        //int summaryLen; //max length for a line of summary
        private Dictionary<String, object> cache;
        QueryResult qr;
        public DBManager(string _tableName, string _descriptionStr,String fname ="memory.sqlite")
        {
            if (SpecialSymbolsFrom == null) DBManager.defineSpecialSymbols(new String[] { "\\","\"","\'","%","&"," ","\t","\n","\r","{","}","[","]","(",")",",",".",";",":","!","?"});
            sql = new SQLiteConnection("data source=" + fname);
            Online = true;
            cmd = new SQLiteCommand(sql);
            reader = null;
            tableName = _tableName;
            descriptionStr = _descriptionStr;
            cache = new Dictionary<string, object>();
        }
        private static void defineSpecialSymbols(string[] symbols)
        {
            int idx = 0;
            SpecialSymbolsFrom = new string[symbols.Length];
            SpecialSymbolsTo = new string[symbols.Length];
            foreach (String symb in symbols)
            {
                SpecialSymbolsFrom[idx] = symb;
                SpecialSymbolsTo[idx] = "$SYM" + idx.ToString() + "$";
                idx++;
            }
        }
        //define fields in the table
        public void setFields(String[] _fnames,FieldType[] _ftypes,string _dfield,string[] _sfield,int _slen=50)
        {
            //ID is automatically added
            numFields = _fnames.Length+1;
            requiredDataFields = _fnames;
            dataField = _dfield;
            fields = new String[numFields];
            fieldTypes = new FieldType[numFields];
            mapField2Type = new Dictionary<string, FieldType>(numFields);
            fields[0] = defaultIdField; fieldTypes[0] = FieldType.id;
            mapField2Type.Add(defaultIdField, FieldType.id);
            for (int idx=1;idx<numFields;idx++)
            {
                fields[idx] = _fnames[idx - 1];
                fieldTypes[idx] = _ftypes[idx - 1];
                mapField2Type.Add(fields[idx], fieldTypes[idx]);
            }
            qr = new QueryResult(mapField2Type,_dfield,_sfield,_slen);
        }
        private void doNoQuery(String s)
        {
            cmd.CommandText = s;
            cmd.ExecuteNonQuery();
        }
        private int readData(String s,bool raw=false)
        {
            cmd.CommandText = s;
            using (reader = cmd.ExecuteReader())
            {
                qr.ReadFrom(reader,raw);
            }
            return qr.Count;
        }
        private bool _online;
        public bool Online
        {
            set { _online = value; if (_online) sql.Open(); else sql.Close(); }
            get { return _online; }
        }
        public void CreateTable()
        {
            String cmd = "create table if not exists " + tableName + " (";
            String s;
            for(int idx=0;idx<numFields;idx++)
            {
                s = "";
                switch(fieldTypes[idx])
                {
                    case FieldType.id:
                        s = fields[idx] + " integer primary key autoincrement";
                        break;
                    case FieldType.int_:
                        s = ", " + fields[idx] + " int default 0";
                        break;
                    case FieldType.float_:
                        s = ", " + fields[idx] + " float default 0";
                        break;
                    case FieldType.text:
                        s = ", " + fields[idx] + " char(256)";
                        break;
                    case FieldType.longtext:
                        s = ", " + fields[idx] + " varchar(32767)";
                        break;
                }
                cmd += s;
            }
            cmd += ")";// character set utf8 collate utf8_general_ci";
            doNoQuery(cmd);
        }
        public void DropTable()
        {
            doNoQuery("drop table " + tableName);
        }
        public QueryResult FilterData(String filterStr="",bool raw=false)
        {
            String cmd = "Select * from " + tableName;
            if(filterStr!="")
            {
                String[] parts = filterStr.Split(';');
                String[] filterPair;
                String currentKey, currentValue;
                bool isFirst = true;
                foreach(String p in parts)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        cmd += " where ";
                    }
                    else
                    {
                        cmd += " and ";
                    }
                    filterPair = p.Split(':');
                    if(filterPair.Length==1)
                    {
                        currentKey = defaultNameField;
                        currentValue = filterPair[0];
                    }
                    else
                    {
                        currentKey = filterPair[0];
                        currentValue = filterPair[1];
                    }
                    if(mapField2Type[currentKey]==FieldType.text||mapField2Type[currentKey]==FieldType.longtext)
                    {
                        currentValue = toDBString(currentValue);
                        cmd += " ( "+currentKey + " like \'%" + currentValue;
                        cmd += "\' or " + currentKey + " like \'%" + currentValue + "%\' ";
                        cmd += " or " + currentKey + " like \'" + currentValue + "%\') ";
                    }
                    else
                    {
                        cmd += currentKey + " " + currentValue;
                    }
                }
            }
            readData(cmd,raw);
            return qr;
        }

        public String FindMatch(String filterStr = "")
        {
            String sqlcmd = "Select * from " + tableName;
            string matched="";
            if (filterStr != "")
            {
                if (cache.ContainsKey(filterStr)) return cache[filterStr].ToString();
                String[] parts = filterStr.Split(';');
                String[] filterPair;
                String currentKey, currentValue;
                bool isFirst = true;
                foreach (String p in parts)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        sqlcmd += " where ";
                    }
                    else
                    {
                        sqlcmd += " and ";
                    }
                    filterPair = p.Split(':');
                    if (filterPair.Length == 1)
                    {
                        currentKey = defaultNameField;
                        currentValue = filterPair[0];
                    }
                    else
                    {
                        currentKey = filterPair[0];
                        currentValue = filterPair[1];
                    }
                    if (mapField2Type[currentKey] == FieldType.text || mapField2Type[currentKey] == FieldType.longtext)
                    {
                        currentValue = toDBString(currentValue);
                        sqlcmd += currentKey + " = \'" + currentValue+ "\'";
                    }
                    else
                    {
                        sqlcmd += currentKey + " " + currentValue;
                    }
                }
            }
            cmd.CommandText = sqlcmd;
            using (SQLiteDataReader tmpRead = cmd.ExecuteReader())
            {
                if(!tmpRead.Read()) return null;
                matched = tmpRead[dataField].ToString();
            }
            matched = fromDBString(matched);
            cache[filterStr] = matched;
            return matched;
        }

        public void AppendData(Dictionary<String,object> entry)
        {
            String vstring,cmd = "Insert into " + tableName;
            bool isFirst = true;
            foreach(KeyValuePair<String,object> kv in entry)
            {
                if (kv.Key== defaultIdField) continue;
                if(isFirst)
                {
                    isFirst = false;
                    cmd += "( ";
                }
                else
                {
                    cmd += ", ";
                }
                cmd += kv.Key;
            }
            cmd += ") values";
            isFirst = true;
            foreach (KeyValuePair<String, object> kv in entry)
            {
                if (kv.Key == defaultIdField) continue;
                if (isFirst)
                {
                    isFirst = false;
                    cmd += "( ";
                }
                else
                {
                    cmd += ", ";
                }
                vstring = kv.Value.ToString();
                if(mapField2Type[kv.Key]==FieldType.longtext||mapField2Type[kv.Key]==FieldType.text)
                {
                    vstring = "\'"+toDBString(vstring)+ "\'";
                }
                cmd += vstring;
            }
            cmd += ")";
            doNoQuery(cmd);
            cache[entry[defaultNameField].ToString()] = entry[dataField];
        }

        public void UpdateData(String indexInDB,Dictionary<String, object> entry)
        {
            String vstring, cmd = "update " + tableName;
            bool isFirst = true;
            foreach (KeyValuePair<String, object> kv in entry)
            {
                if (isFirst)
                {
                    isFirst = false;
                    cmd += " set ";
                }
                else
                {
                    cmd += ", ";
                }
                cmd += kv.Key+" = ";
                vstring = kv.Value.ToString();
                if (mapField2Type[kv.Key] == FieldType.longtext || mapField2Type[kv.Key] == FieldType.text)
                {
                    vstring = "\'" + toDBString(vstring) + "\'";
                }
                cmd += vstring;
            }
            cmd += " where "+DBManager.defaultIdField+" = "+indexInDB;
            doNoQuery(cmd);
            cache[entry[defaultNameField].ToString()] = entry[dataField];
        }

        public void DeleteData(String indexInDB)
        {
            String cmd = "delete from " + tableName;
            cmd += " where " + DBManager.defaultIdField + " = " + indexInDB;
            doNoQuery(cmd);
        }
        public void DeleteMatch(String nameInDB)
        {
            String cmd = "delete from " + tableName;
            cmd += " where " + DBManager.defaultNameField + " = " + nameInDB;
            doNoQuery(cmd);
        }

        public override string ToString()
        {
            String s = tableName + ":\n";
            s += descriptionStr + "\n";
            foreach(KeyValuePair<String,FieldType> kv in mapField2Type)
            {
                s += "\t" + kv.Key + ": " + kv.Value + "\n";
            }
            return s;
        }

        public static String toDBString(String s)
        {
            for (int idx = 0; idx < SpecialSymbolsFrom.Length; idx++)
                s = s.Replace(SpecialSymbolsFrom[idx], SpecialSymbolsTo[idx]);
            return s;
        }
        public static String fromDBString(String s)
        {
            for (int idx = 0; idx < SpecialSymbolsFrom.Length; idx++)
                s = s.Replace(SpecialSymbolsTo[idx], SpecialSymbolsFrom[idx]);
            return s;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    class DBGroup
    {
        private Dictionary<String, DBManager> db;   //database managers
        private QueryResult currentRead;
        private string currentTName,fname;
        public DBGroup(String[] tables,String _fname="memory.sqlite")
        {
            db = new Dictionary<string, DBManager>();
            fname = _fname;
            currentRead = null;
            currentTName = null;
        }
        public String[] ReqDataFields(String tname="")
        {
            if(tname=="") return db[currentTName].requiredDataFields;
            if (!(db.ContainsKey(tname))) return new string[]{ };
            return db[tname].requiredDataFields;
        }

        public String InitDB(String tname,String description,String[] fields,DBManager.FieldType[] types,string[] akas,string _dfield,string[] _sfield,int _slen=50)
        {
            db.Add(tname, new DBManager(tname,description, fname));
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            db[tname].setFields(fields, types,_dfield,_sfield,_slen);
            db[tname].CreateTable();
            foreach(string aka in akas)
            {
                db.Add(aka, db[tname]);
            }
            return tname + " created.";
        }
        public String Description(String tname)
        {
            if (tname == "") return ToString();
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            return db[tname].ToString();
        }
        public String AddData(String tname,Dictionary<String,object> entry)
        {
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            db[tname].AppendData(entry);
            return "Added to "+tname + ".";
        }
        public String UpdateData(String tname, String index,Dictionary<String, object> entry)
        {
            if (tname == "") tname = currentTName;
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            db[tname].UpdateData(index,entry);
            return "Updated " + tname + ".";
        }
        public String DeleteData(String tname, String index)
        {
            if (tname == "") tname = currentTName;
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            db[tname].DeleteData(index);
            return "Deleted from " + tname + ".";
        }
        public String DropTable(String tname)
        {
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            db[tname].DropTable();
            db.Remove(tname);
            db["tables"].DeleteMatch(tname);
            return tname + " dropped.";
        }
        public String exportTable(String tname,String fname, String fmt)
        {
            if (!(db.ContainsKey(tname))) throw new Exception("Error! " + tname + " Not Found!");
            QueryResult qr = db[tname].FilterData("",true);
            FileDao.ToTXT(qr.currentRead, fname);
            return "Success.";
        }
        public String importTable(String tname,String fname,String fmt)
        {
            List<Dictionary<String, Object>> data = FileDao.FromTXT(fname);
            String ret = "";
            if (!(db.ContainsKey(tname))) throw new Exception("Error! " + tname + " Not Found!");
            foreach(Dictionary<String, Object> dict in data)
            {
                try { db[tname].AppendData(dict); }
                catch(Exception ex)
                {
                    ret += "Error! " + ex + "\n";
                }
            }
            return ret+"Successfully loaded data into "+tname+".";
        }
        public String FindInTable(String tname,String filterString="")
        {
            if (tname == "") tname = currentTName;
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            QueryResult qr=db[tname].FilterData(filterString);
            currentRead = qr;
            currentTName = tname;
            return qr.ToString();
        }
        public String FindMatch(String tname, String filterString = "")
        {
            if (tname == "") tname = currentTName;
            if (!(db.ContainsKey(tname))) return "Error! " + tname + " Not Found!";
            return db[tname].FindMatch(filterString);
        }
        public String CopyData(int idx=-1)
        {
            if (currentRead == null) return "";
            return currentRead.Copy(idx);
        }
        public void InitTable(String tableName,String desc,String _content)
        {
            string[] typeStr,content;
            content = _content.Replace("\r","").Split('\n');
            DBManager.FieldType[] types;
            typeStr = content[1].Split(';');
            types = new DBManager.FieldType[typeStr.Length];
            for (int idx = 0; idx < typeStr.Length; idx++)
            {
                switch (typeStr[idx])
                {
                    case "int":
                        types[idx] = DBManager.FieldType.int_;
                        break;
                    case "float":
                        types[idx] = DBManager.FieldType.float_;
                        break;
                    case "id":
                        types[idx] = DBManager.FieldType.id;
                        break;
                    case "text":
                        types[idx] = DBManager.FieldType.text;
                        break;
                    case "longtext":
                        types[idx] = DBManager.FieldType.longtext;
                        break;
                }
            }
            try
            {
                InitDB(
                    tableName, desc,content[0].Split(';'),
                    types, content[2].Split(';'),
                    content[3], content[4].Split(';')
                );
            }
            catch
            {
                try
                {
                    db["tables"].DeleteMatch(tableName);
                    db.Remove(tableName);
                }
                catch { }
                finally { }
            }
        }
        public void InitTables(String table="table")
        {
            //Initialize dynamic from tables
            QueryResult qr = db[table].FilterData();
            string tableName, content;
            string desc;
            foreach(Dictionary<String,object> t0 in qr.currentRead)
            {
                tableName = t0[DBManager.defaultNameField].ToString();
                desc = t0["desc"].ToString();
                content = t0[db[table].dataField].ToString();
                InitTable(tableName, desc, content);
                
            }
        }
        public String Backup()
        {
            try { File.Copy(fname, fname + "backup"); }
            catch(Exception ex) { return ex.ToString(); }
            return "Success";
        }
        public String UseBackup()
        {
            if (!File.Exists(fname + "backup")) return "No backups found!";
            setOnline(false);
            File.Copy(fname, fname + "tmp");
            File.Delete(fname);
            File.Move(fname + "backup", fname );
            File.Move(fname + "tmp", fname + "backup");
            setOnline(true);
            return "Success!";
        }
        public Dictionary<String,object> Check(ref int idx)
        {
            return currentRead.Check(ref idx);
        }
        public void setOnline(bool online)
        {
            foreach(DBManager db0 in db.Values)
            {
                db0.Online=online;
            }
        }
        public override string ToString()
        {
            String s = "Tables:\n";
            foreach(KeyValuePair<String,DBManager> kv in db)
            {
                s += "\t" + kv.Key + ": " + kv.Value.ToString() + "\n";
            }
            return s;
        }
    }
}

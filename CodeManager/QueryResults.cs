using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace CodeManager
{
    class QueryResult
    {
        Dictionary<String, DBManager.FieldType> mapField2Type;
        public List<Dictionary<String, object>> currentRead;
        public int currentReadIdx;
        string dataField;   //default field for core data(to be copied)
        string[] summaryFields; //default fields for list summary
        int summaryLen; //max length for a line of summary
        public int Count
        {
            get
            {
                if (mapField2Type == null) return -1;
                return mapField2Type.Count;
            }
        }
        public Dictionary<String,object> Check(ref int idx)
        {
            if (idx < 0 || idx > currentRead.Count)
            {
                if (currentReadIdx < 0) currentReadIdx = 0;
                idx = currentReadIdx;
                return currentRead[currentReadIdx];
            }
            currentReadIdx=idx;
            return currentRead[idx];
        }
        public String Copy(int idx)
        {
            if (idx < 0 || idx > currentRead.Count) return currentRead[currentReadIdx][dataField].ToString();
            return currentRead[idx][dataField].ToString();
        }
        public String Index(int idx)
        {
            if (idx < 0 || idx > currentRead.Count) return currentRead[currentReadIdx][DBManager.defaultIdField].ToString();
            return currentRead[idx][DBManager.defaultIdField].ToString();
        }
        public QueryResult(Dictionary<String, DBManager.FieldType> _mapField2Type,string _dfield,string[] _sfield,int _slen)
        {
            mapField2Type = _mapField2Type;
            currentReadIdx = 0;
            dataField = _dfield;
            summaryFields = _sfield;
            summaryLen = _slen;
        }
        public void ReadFrom(SQLiteDataReader reader)
        {
            currentRead = new List<Dictionary<string, object>>();
            Dictionary<String, object> tmp;
            String itm;
            while(reader.Read())
            {
                tmp = new Dictionary<string, object>(mapField2Type.Count);
                foreach(KeyValuePair<String, DBManager.FieldType> kv in mapField2Type)
                {
                    itm = reader[kv.Key].ToString();
                    switch (kv.Value)
                    {
                        case DBManager.FieldType.id:
                        case DBManager.FieldType.int_:
                            tmp.Add(kv.Key, int.Parse(itm));
                            break;
                        case DBManager.FieldType.float_:
                            tmp.Add(kv.Key, float.Parse(itm));
                            break;
                        case DBManager.FieldType.text:
                        case DBManager.FieldType.longtext:
                            tmp.Add(kv.Key, DBManager.fromDBString(itm));
                            break;
                    }
                }
                currentRead.Add(tmp);
            }
            currentReadIdx = 0;
        }
        //Generate summary of the current results.
        public override string ToString()
        {
            string s = "Results:\n";
            string line;
            int idx = 0;
            foreach (Dictionary<String, object> d in currentRead)
            {
                line = "\t" + idx.ToString()+":\t";
                idx++;
                foreach(String k in summaryFields)
                {
                    line += (d[k].ToString().Replace('\n','\t')+"\t");
                }
                if (line.Length > summaryLen)
                    line = line[0] + line.Substring(1, summaryLen - 2) + "\n";
                else line = line + "\n";
                s += line;
            }
            return s;

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    class FileDao
    {
        public static String ToTXT(List<Dictionary<String, Object>> lt,String fname)
        {
            String line, itm;
            using (StreamWriter stream = new StreamWriter(fname,false, Encoding.UTF8))
            {
                foreach(Dictionary<String,Object> t in lt)
                {
                    line = "";
                    foreach(KeyValuePair<String,Object> kv in t)
                    {
                        itm = kv.Key+"%"+kv.Value.ToString();
                        line += itm + "|";
                    }
                    stream.Write(line + "\n");
                }
            }
            return "Success.";
        }
        public static List<Dictionary<String, Object>> FromTXT(String fname)
        {
            List<Dictionary<String, Object>> ret;
            String fContent;
            using (StreamReader stream = new StreamReader(fname,Encoding.UTF8))
            {
                ret = new List<Dictionary<string, object>>();
                fContent=stream.ReadToEnd();
                foreach(String line in fContent.Split('\n'))
                {
                    if (line.Trim() == "") continue;
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    foreach (String itm in line.Trim().Split('|'))
                    {
                        if (itm == "") continue;
                        String[] tmp = itm.Split('%');
                        if (tmp.Length < 2) continue;
                        d.Add(tmp[0], tmp[1]);
                    }
                    ret.Add(d);
                }
                return ret;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeManager
{
    public partial class FrmInteract:Form
    {
        private String dropTable(String[] name)
        {
            return db.DropTable(name[0]);
        }
        //User adds a table
        private String newTable(String[] name)
        {
            string title;
            if (name.Length > 0) title = name[0];
            else title = "";
            frmedit.startEdit(title);
            if (frmedit.confirmed)
            {
                db.AddData("table", frmedit.ToDictionary(db.ReqDataFields("table")));
                db.InitTable(frmedit.Title, frmedit.Desc, frmedit.Code);
                return "Completed.";
            }
            return "Canceled.";
        }
        private String save(String[] name)
        {
            string dbname, title, content;
            if (name.Length > 0) dbname = name[0];
            else return "DBName required!";
            if (name.Length > 1) title = name[1];
            else title = "";
            if (name.Length > 2) content = name[2];
            else content = "";
            if ((dbname == "code" || dbname == "codes") && content != "") content = loadText(currentPath + "\\" + content.Replace('/', '\\'));
            else if (dbname == "path" || dbname == "paths") content = currentPath + "\\" + content.Replace('/', '\\');
            frmedit.startEdit(title, "", content);
            if (frmedit.confirmed)
            {
                db.AddData(dbname, frmedit.ToDictionary(db.ReqDataFields(dbname)));
                return "Completed.";
            }
            return "Canceled.";
        }
        private String find(String[] name)
        {
            string dbname, filterString;
            if (name.Length > 0) dbname = name[0];
            else return "DBName required!";
            if (name.Length > 1) filterString = name[1];
            else filterString = "";
            return db.FindInTable(dbname, filterString);
        }
        private String copy(String[] name)
        {
            int idx = check(name);
            if (idx > -1)
            {
                String s = db.CopyData(idx);
                if (name.Length > 1)
                {
                    String fname = currentPath + "\\" + name[1].Replace('/', '\\');
                    using (StreamWriter sw = new StreamWriter(fname, true))
                    {
                        sw.Write(s);
                    }
                    return "Written to " + fname;
                }
                else
                {
                    Clipboard.SetText(s);
                    return "Copied to clipboard.";
                }
            }
            else return "Canceled.";
        }
        
        

        private String update(String[] name)
        {
            int idx = -1;
            String indexInDB;
            if (name.Length > 0) idx = int.Parse(name[0]);
            Dictionary<string, object> d = db.Check(ref idx);
            indexInDB = d[DBManager.defaultIdField].ToString();
            string[] fields = db.ReqDataFields();
            frmedit.startEdit(d[fields[0]].ToString(), d[fields[1]].ToString(), d[fields[2]].ToString());
            if (frmedit.confirmed)
            {
                d = frmedit.ToDictionary(fields);
                return db.UpdateData("", indexInDB, d);
            }
            else
            {
                return "Canceled.";
            }
        }
        
        private String delete(String[] name)
        {
            int idx = -1;
            String indexInDB;
            if (name.Length > 0) idx = int.Parse(name[0]);
            Dictionary<string, object> d = db.Check(ref idx);
            indexInDB = d[DBManager.defaultIdField].ToString();
            string[] fields = db.ReqDataFields();
            frmedit.startEdit(d[fields[0]].ToString(), d[fields[1]].ToString(), d[fields[2]].ToString());
            if (frmedit.confirmed)
            {
                return db.DeleteData("", indexInDB);
            }
            else
            {
                return "Canceled.";
            }
        }
        private String changeDir(String[] name)
        {
            if (name.Length > 1) currentPath = name[0].Replace('/', '\\');
            else if (name.Length == 1) currentPath += "\\" + name[0].Replace('/', '\\');
            return "Current Path: " + currentPath;
        }
        private String calculation(String[] name)
        {
            return name[0] + " = " + dt.Compute(name[0], "false").ToString();
        }

        private String startExplorer(String[] name)
        {
            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo("explorer.exe");
            if (name.Length > 0) psi.Arguments = name[0];
            else psi.Arguments = currentPath;
            if (name.Length > 1) psi.Arguments += ",/select" + name[1];
            currentPath = name[0];
            p.StartInfo = psi;
            p.Start();
            return "";
        }

        private String doStartUp(string fname = "")
        {
            if (fname == "") fname = "startup.ini";
            string s = "StartUp. " + fname + "\n";
            try
            {
                foreach (string line in loadText(fname).Split('\n'))
                {
                    s += handler.handle(line) + "\n";
                }
            }
            catch
            {
                return "";
            }
            return s;
        }
        private string editStartUp(string[] name)
        {
            string fname = "startup.ini";
            frmedit.startEdit(fname, "Startup commands", loadText(fname));
            if (!frmedit.confirmed) return "Canceled.";
            using (StreamWriter sw = new StreamWriter(fname))
            {
                sw.Write(frmedit.Code);
            }
            return "Modified " + fname;
        }
        private string editGIFs(string[] name)
        {
            string fname = "gifs.ini";
            frmedit.startEdit(fname, "Startup GIFs", loadText(fname));
            if (!frmedit.confirmed) return "Canceled.";
            using (StreamWriter sw = new StreamWriter(fname))
            {
                sw.Write(frmedit.Code);
            }
            parent.initSelectableGIFs(fname);
            return "Modified " + fname + ".";
        }
        private string retCurretPath(string[] name)
        {
            return "Current Path: " + currentPath;
        }
    }
}

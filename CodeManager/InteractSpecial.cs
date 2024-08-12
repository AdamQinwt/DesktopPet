using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    public partial class FrmInteract
    {
        //directly run a command from query results or open a file
        //Format: exe_filterString args
        private String run(String[] name)
        {
            try
            {
                String cmd, arguments, s = "Running.\n", tmpstr;
                int idx;
                String[] scripts;
                Process p;
                bool isCMD = false;
                try { scripts = db.FindMatch("exec", name[0]).Split('\n'); }
                catch { scripts = db.FindMatch("path", name[0]).Split('\n'); }
                if (scripts.Length < 1) return "No command Found!";
                foreach (String s0 in scripts)
                {
                    p = new Process();
                    cmd = "";
                    arguments = "";
                    foreach (String itm in s0.Split('|'))
                    {
                        if (cmd == "")
                        {
                            cmd = itm;
                            if (cmd.Contains("$CMD$"))
                            {
                                isCMD = true;
                                p.StartInfo.RedirectStandardError = true;
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.RedirectStandardInput = true;
                                p.StartInfo.RedirectStandardOutput = true;
                                cmd = cmd.Replace("$CMD$", "");
                            }
                            if (cmd[0] == '#')
                            {
                                tmpstr = db.FindMatch("path", cmd.Remove(0, 1));
                                if(tmpstr == "") tmpstr = db.FindMatch("path", cmd.Remove(0, 1));
                                cmd = tmpstr;
                            }
                        }
                        else
                        {
                            if (itm == "") break;
                            tmpstr = itm;
                            for (idx = 1; idx < name.Length; idx++)
                            {
                                tmpstr = tmpstr.Replace("$ARG" + idx.ToString() + "$", name[idx]);
                            }
                            for (; idx < 20; idx++)
                            {
                                tmpstr = tmpstr.Replace("$ARG" + idx.ToString() + "$", "");
                            }
                            if (tmpstr != "")
                            {
                                if (tmpstr[0] == '#')
                                {
                                    tmpstr = db.FindMatch("path", tmpstr.Remove(0, 1));
                                }
                            }
                            arguments += tmpstr + " ";
                        }
                    }

                    s += "\t" + cmd + " " + arguments + "\n";
                    p.StartInfo.FileName = cmd;
                    p.StartInfo.Arguments = arguments;
                    p.Start();
                    if (isCMD)
                    {
                        p.WaitForExit();
                        s += p.StandardOutput.ReadToEnd() + "\n";
                        p.Close();
                    }
                }
                return s;
            }
            catch (Exception ex)
            {
                return "Command not Found!\n" + ex.ToString();
            }
        }
        //open a path in explorer
        private String explore(String[] name)
        {
            try
            {
                int idx = check(name);
                if (idx > -1)
                {
                    String s = db.CopyData(idx);
                    startExplorer(new string[] { s });
                    return "Opened.";
                }
                else return "Canceled.";
            }
            catch (Exception ex)
            {
                return startExplorer(name);
            }
        }
        // Show table columns.
        private String describe(String[] name)
        {
            String tname = "";
            if (name.Length > 0) tname = name[0];
            return db.Description(tname);
        }
        private string readProject(string[] name)
        {
            try
            {
                int idx = check(name);
                if (idx > -1)
                {
                    string[] prjParts = db.CopyData(idx).Split('\n'), f0Parts, code0, codeParts;
                    string fname;
                    string ret="Writing project:\n";
                    int codeIdx;
                    foreach (String f0 in prjParts)
                    {
                        f0Parts = f0.Split(':');
                        if (f0Parts.Length == 0) continue;
                        fname = f0Parts[0];
                        codeParts = f0Parts[1].Split(',');
                        code0 = new string[codeParts.Length];
                        codeIdx = 0;
                        foreach(string codeFrag in codeParts)
                        {
                            code0[codeIdx] = db.FindMatch("code", codeFrag.Replace("\r",""));
                            codeIdx++;
                        }
                        ret += writeText(fname, code0, currentPath);
                    }
                    return ret;
                }
                else return "Canceled.";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}

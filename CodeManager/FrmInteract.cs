using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace CodeManager
{
    public enum GlobalCommand
    {
        Normal=0,
        Quit,
        Hide,
        Show,
    }
    public partial class FrmInteract : Form
    {
        private StringBuilder outputs;
        private FrmDisplay parent;
        private FrmEdit frmedit;
        private FrmChat frmchat;
        DataTable dt;
        public String Command
        {
            get
            {
                return txtCmd.Text;
            }
            set
            {
                txtCmd.Text = value;
                try
                {
                    txtCmd.Select(value.Length, 0);
                    txtCmd.ScrollToCaret();
                }
                catch { }
            }
        }
        private CmdHandler handler;
        private GlobalCommand gc;   // Special commands to be handed to FrameShow
        DBGroup db;
        static string welcomeMsg = "Welcome to Aeobro Code Manager!\r\n";
        string currentPath;


        public FrmInteract()
        {
            InitializeComponent();
            initDB();
            currentPath = "e:";
            dt = new DataTable();
            frmedit = new FrmEdit();
            frmchat = new FrmChat();
            handler = new CmdHandler(20);
            outputs = new StringBuilder();
            handler.register("quit", setQuit, "Quit");
            handler.register("hide", setHide, "Hide");
            handler.register("show", setShow, "Show");
            handler.register("save", save, "Save contents. Params: tableName [title] [content(file for code and subpath for path)]", new string[] {"remember","memorize"});
            handler.register("find", find, "Find contents. Params: tableName [filterString(k1:v1;k2:v2;...)]", new string[] {"recall","filter","get","li"});
            handler.register("del", delete, "Delete contents. Params: index(default 0)", new string[] { "delete","remove","forget","-" });
            handler.register("update", update, "Update contents. Params: index(default 0)", new string[] { "fix","change","+","detail" });
            handler.register("describe", describe, "Check table columns. Params: [tableName]", new string[] { "intro","desc" });
            handler.register("drop_table_and_i_confirm", dropTable, "Delete a table. Params: tableName Designed to be long");
            handler.register("create_table", newTable, "Create a table. Params: [tableName]");
            handler.register("font", setFont, "Set or Check Font. Params: [size/font size]]");
            handler.register("calc", calculation, "Direct Calculation.",new string[] {"calculation" });
            handler.register("cmd", startCMD, "Start a cmd console.");
            handler.register("chat", startChat, "Start a chat window.");
            handler.register("explore", explore, "Start an explorer. Params: index(default 0)",new string[]{"folder","goto"});
            handler.register("cd", changeDir, "Change current path. Params: path [isAbsolute]",new string[]{"change_dir"});
            handler.register("copy", copy, "Copy contents. Params: index(default 0)", new string[] { "fetch" });
            handler.register("project", readProject, "Copy contents from a project. Params: index(default 0)", new string[] { "prj","refer" });
            handler.register("cls", clearScreen, "Clear Screen.");
            handler.register("startup", editStartUp, "Edit StartUp Commands.");
            handler.register("gif", editGIFs, "Edit GIF paths.", new string[] { "gifs" });
            handler.register("cpath", retCurretPath, "Shows the current path.");
            handler.registerDefault(run);
            appendOutput(welcomeMsg);
            doStartUp();
        }
        private void FrmInteract_Load(object sender, EventArgs e)
        {
            gc = GlobalCommand.Normal;
        }
        public GlobalCommand startInteract(FrmDisplay p)
        {
            parent = p;
            ShowDialog();
            return gc;
        }
        private void initDB()
        {
            string description;
            //basic tables
            db = new DBGroup(new String[] { "code", "exec", "path" ,"projects"});
            //code: code fragments
            //can be saved directly from a file
            description = "code: code fragments\ncan be saved directly from a file";
            db.InitDB(
                "code",
                description,
                new string[] { "name", "desc", "code" },
                new DBManager.FieldType[] { DBManager.FieldType.text, DBManager.FieldType.text, DBManager.FieldType.longtext },
                new string[] { "codes"},
                "code",
                new string[] { "name", "desc"}
                );
            //exec: executables
            //can be executed directly in the run function
            description = "exec: executables\ncan be executed directly in the run function\n";
            description += "$CMD$ for redirecting outputs.\n| for splitting executable file and params\n ";
            description += "$ARGx$ for the x parameter.\n# for replacing with whatever is in paths or exes";
            db.InitDB(
                "exec",
                description,
                new string[] { "name", "desc", "cmd" },
                new DBManager.FieldType[] { DBManager.FieldType.text, DBManager.FieldType.text, DBManager.FieldType.longtext },
                new string[] { "exe","runnable"},
                "cmd",
                new string[] { "name", "desc" }
                );
            //path: frequently accessed paths
            //can be opened directly in the run function
            //can be modified with explore/changeDir functions
            //can be saved directly from the current path
            description = "path: frequently accessed paths\n";
            description += "can be opened directly in the run function\n";
            description += "can be modified with explore/changeDir functions\n";
            description += "can be saved directly from the current path";
            db.InitDB(
                "path",
                description,
                new string[] { "name", "desc", "path" },
                new DBManager.FieldType[] { DBManager.FieldType.text, DBManager.FieldType.text, DBManager.FieldType.text },
                new string[] { "paths"},
                "path",
                new string[] { "name", "desc", "path" }
                );
            //tables: dynamic tables defined by the user
            //created from the create_table command
            //deleted by drop_table_and_i_confirm
            //recommneded NOT to edit or save or del
            //core contains lines of the following:
            //field names
            //types
            //akas
            //datafield
            //summary field
            description = "tables: dynamic tables defined by the user\n";
            description += "created from the create_table command\n";
            description += "deleted by drop_table_and_i_confirm\n";
            description += "recommneded NOT to edit or save or del\n";
            description += "core contains lines of the following:\n";
            description += "\tfield names\n";
            description += "\ttypes\n";
            description += "\takas\n";
            description += "\tdata field\n";
            description += "\tsummary field";
            db.InitDB(
                "tables",
                description,
                new string[] { "name", "desc", "core" },
                new DBManager.FieldType[] { DBManager.FieldType.text, DBManager.FieldType.text, DBManager.FieldType.longtext },
                new string[] { "tb","table" },
                "core",
                new string[] { "name", "desc", "core" }
            );
            db.InitTables("tables");
            //projects: sets of codes to be copied or imported together
            //refer: each line corresponds to a code fragment
            //create project: makedir, save to path, copy code to files.
            //read: get all fragments and save to files respectively
            description = "projects: sets of codes to be copied or imported together\n";
            description += "refer: each line corresponds to a code fragment\n";
            description += "filename:codeFragment1[;codeFragment2[...]]\n";
            description += "create project: makedir, save to path, copy code to files.\n";
            description += "read: get all fragments and save to files respectively.\n";
            db.InitDB(
                "projects", //sets of codes
                description,
                new string[] { "name", "desc", "refer" },
                new DBManager.FieldType[] { DBManager.FieldType.text, DBManager.FieldType.text, DBManager.FieldType.longtext },
                new string[] { "prj", "proj" },
                "refer",
                new string[] { "name", "desc" }
            );
        }
        private void appendOutput(String s)
        {
            if (s != "")
            {
                outputs.Append(s + "\n");
                txtHistory.Text = outputs.ToString().Replace("\r", "").Replace("\n", "\r\n");
            }
            txtHistory.Select(txtHistory.Text.Length, 0);
            txtHistory.ScrollToCaret();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape: Hide(); return true;
                case Keys.Enter:
                    if (txtCmd.Focused)
                    {
                        appendOutput("User: "+Command);
                        appendOutput(handler.handle(Command));
                        Command = "";
                        return true;
                    }
                    return false;
                case Keys.Up:
                    if (txtCmd.Focused)
                    {
                        Command = handler.last();
                        return true;
                    }
                    return false;
                case Keys.Down:
                    if (txtCmd.Focused)
                    {
                        Command = handler.last();
                        return true;
                    }
                    return false;
                default: break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private int check(string[] _idx)
        {
            int idx = -1;
            if (_idx.Length > 0) idx = int.Parse(_idx[0]);
            Dictionary<string, object> d = db.Check(ref idx);
            string[] fields = db.ReqDataFields();
            frmedit.startEdit(d[fields[0]].ToString(), d[fields[1]].ToString(), d[fields[2]].ToString());
            return frmedit.confirmed ? idx : -1;
        }
        private String setQuit(String[] name)
        {
            gc = GlobalCommand.Quit;
            parent.exit();
            return "";
        }
        private String setHide(String[] name)
        {
            gc = GlobalCommand.Hide;
            //hide();
            parent.Hide();
            return "";
        }
        private String setShow(String[] name)
        {
            gc = GlobalCommand.Show;
            parent.Show();
            return "";
        }
        private void hide()
        {
            Hide();
        }
        private String clearScreen(String[] name)
        {
            outputs = new StringBuilder();
            appendOutput(welcomeMsg);
            return "";
        }
        private String setFont(String[] name)
        {
            if (name.Length == 0) return txtHistory.Font.ToString();
            if (name.Length == 1) txtHistory.Font = new Font(txtHistory.Font.FontFamily, float.Parse(name[0]));
            else txtHistory.Font = new Font(name[0], float.Parse(name[1]));
            return "Setting font to " + txtHistory.Font.ToString();
        }
        private String startCMD(String[] name)
        {
            Process.Start("cmd");
            return "";
        }
        private String startChat(String[] name)
        {
            frmchat.startChat();
            return "";
        }
        private void txtCmd_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {

            }
        }
        private String loadText(string fname = "")
        {
            if (fname == "")
                return "";
            string s = "";
            try
            {
                using (StreamReader sr = new StreamReader(fname))
                    s = sr.ReadToEnd();
            }
            catch
            {
                //
            }
            return s;
        }
        private string writeText(string fname,string[] s,string root="")
        {
            string ret = "";
            try
            {
                using (StreamWriter sr = new StreamWriter(root+"/"+fname))
                {
                    foreach (string s0 in s)
                    {
                        try
                        {
                            sr.Write(s0 + "\n");
                            ret += s0.Length.ToString() + " char written.\n";
                        }
                        catch(Exception ex)
                        {
                            ret += "Cannot write a part to " + fname + "\n" + ex.ToString() + "\n";
                        }
                    }
                }
                ret += "Written to " + fname + "\n";
            }
            catch(Exception ex)
            {
                ret += "Cannot write " + fname + "\n" + ex.ToString() + "\n";
            }
            return ret;
        }
        
    }
}

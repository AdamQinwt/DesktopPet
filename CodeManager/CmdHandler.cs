using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    class CmdHandler
    {
        CmdHistory history;
        public delegate String HandleCommand(String[] args);
        Dictionary<String, HandleCommand> funcs;
        Dictionary<String, String> helps;
        HandleCommand defaultFunc;
        public CmdHandler(int historySize=20)
        {
            history = new CmdHistory(historySize);
            funcs = new Dictionary<String, HandleCommand>();
            helps = new Dictionary<String, String>();
            register("help", displayHelp, "Show helps and hints.",new string[] { "?"});
            register("history", displayHistory, "Show history. Params: [filters].", new string[] { "hist" });
            register("repeat_history", repeatHistory, "Repeat an history entry. Params: idx", new string[] {"repeat"});
        }
        public void register(String cmd,HandleCommand func,String help,string[] akas=null)
        {
            _register(cmd, func, help);
            if (akas != null)
            {
                foreach (String aka in akas) _register(aka, func, help);
            }
        }
        public void registerDefault(HandleCommand func)
        {
            defaultFunc = func;
        }
        private void _register(String cmd, HandleCommand func, String help)
        {
            funcs.Add(cmd, func);
            helps.Add(cmd, help);
        }
        public String handle(String cmd)
        {
            if (cmd == "") return "";
            try
            {
                String r = "";
                String[] parts = cmd.Split(' ');
                if (parts[0] != "history" && parts[0] != "hist") history.append(cmd);
                if (funcs.ContainsKey(parts[0])) return funcs[parts[0]](parts.Skip(1).ToArray());
                else if (defaultFunc != null)
                {
                    return defaultFunc(parts);
                }
                else r = parts[0] + ": " + "Command Not Found!!";
                return r;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
        public String last() { return history.last(); }
        public String next() { return history.next(); }
        public String displayHelp(String[] name)
        {
            String r = "Help:\n";
            if(name.Length<1)
            {
                foreach(KeyValuePair<String,String> help0 in helps)
                {
                    r += "\t"+help0.Key+": " + help0.Value + "\n";
                }
            }
            else
            {
                foreach(String name0 in name)
                {
                    r += "\t" + name0 + ": ";
                    if (helps.ContainsKey(name0)) r += helps[name0];
                    else r += "Not Found!";
                    r += "\n";
                }
            }
            return r;
        }
        private String displayHistory(String[] name)
        {
            String r = "History:\n";
            if (name.Length < 1) history.dump();
            else history.dump(name[0]);
            for(int idx=0;idx<history.filteredCnt;idx++)
            {
                r += "\t"+idx.ToString()+": " + history.currentHistory[idx] + "\n";
            }
            return r;
        }
        private String repeatHistory(String[] name)
        {
            if (name.Length < 1) return "Error! Index required!!";
            try
            {
                String cmd = history.getFiltered(Int32.Parse(name[0]));
                if (cmd == "") return "Empty Command.";
                return handle(cmd);
            }catch(Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}

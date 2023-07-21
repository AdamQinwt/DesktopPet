using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeManager
{
    class Chatter
    {
        string url;
        WebClient wc;
        Dictionary<string, object> chatDict,chatRet;
        List<List<String>> history;
        CmdHistory cmdHistory;
        public Chatter(string _url=null)
        {
            if (_url != null) url = _url;
            else url = "";
            wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            history = new List<List<String>>();
            chatDict = new Dictionary<string, object>(5);
            chatDict.Add("history", history);
            chatDict.Add("max_length", 2048);
            chatDict.Add("prompt", "");
            chatDict.Add("top_p", .7);
            chatDict.Add("temperature", .95);
            cmdHistory = new CmdHistory(10);
        }
        public String chat(string s)
        {
            string r;
            chatDict["prompt"] = s;
            byte[] response = wc.UploadData(url, "post", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chatDict)));
            r = Encoding.UTF8.GetString(response);
            chatRet = JsonConvert.DeserializeObject<Dictionary<String, object>>(r);
            r = (string)chatRet["response"];
            history.Add(new List<string>(new string[] {s,r}));
            chatDict["history"] = history;
            return r;
        }
        public String last() { return cmdHistory.last(); }
        public String next() { return cmdHistory.next(); }
    }
}

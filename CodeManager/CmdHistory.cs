using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeManager
{
    class CmdHistory
    {
        private String[] history;
        private int size,currentIdx,appendIdx;
        public int filteredCnt;
        public String[] currentHistory; //filtered history
        public CmdHistory(int _size=20)
        {
            size = _size;
            history = new String[_size];
            for (int idx = 0; idx < _size; idx++) history[idx] = "";
            currentIdx = 0;
            appendIdx = 0;
        }
        public void append(String s)
        {
            history[appendIdx] = s;
            appendIdx++;
            while (appendIdx >= size) appendIdx -= size;
            currentIdx = appendIdx;
        }
        public String at(int idx)
        {
            while (idx >= size) idx -= size;
            while (idx < 0) idx += size;
            return history[idx];
        }
        public String last()
        {
            currentIdx--;
            return at(currentIdx);
        }
        public String next()
        {
            currentIdx++;
            return at(currentIdx);
        }
        public String[] dump(String filter=null)
        {
            String[] tmp = new String[size];
            int idx=0;
            foreach(String s in history)
            {
                if (s == "") continue;
                if(filter!=null)
                {
                    if (!s.Contains(filter)) continue;
                }
                tmp[idx] = s;
                idx++;
            }
            filteredCnt = idx;
            currentHistory = tmp;
            return tmp;
        }
        public String getFiltered(int idx)
        {
            if (idx < 0 || idx >= filteredCnt) return "";
            return currentHistory[idx];
        }
    }
}

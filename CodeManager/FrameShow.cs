using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CodeManager
{
    public partial class FrmDisplay : Form
    {
        FrmInteract frmInteract;
        List<string> selectableGIFs;
        int currentGIFIdx,swapGIFInterval;
        bool isDragging;
        Point dragStart,windowLoc;
        Random random;
        int m_nScrWidth, m_nScrHeight;
        public FrmDisplay()
        {
            InitializeComponent();
            checkRunning();
            random = new Random();
            frmInteract = new FrmInteract();
            isDragging = false;
            m_nScrWidth = Screen.PrimaryScreen.Bounds.Width;
            m_nScrHeight = Screen.PrimaryScreen.Bounds.Height;
            currentGIFIdx = 0;
            swapGIFInterval = -1;
            initSelectableGIFs("gifs.ini");
        }
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool SetFocus(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd,int nCmdShow);
        [DllImport("user32.dll")]
        private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        private void checkRunning(string name="CodeManager")
        {
            int currentId = Process.GetCurrentProcess().Id;
            Process[] processes = Process.GetProcessesByName(name);
            if (processes.Length > 0)
            {
                foreach(Process tmp in processes)
                {
                    if (tmp.Id == currentId) continue;
                    IntPtr handle = processes[0].MainWindowHandle;
                    SetFocus(handle);
                    SetForegroundWindow(handle);
                    ShowWindow(handle,5);
                    SwitchToThisWindow(handle, true);
                    Environment.Exit(0);
                }
            }
        }
        private String interact()
        {
            GlobalCommand gcmd=frmInteract.startInteract(this);
            switch (gcmd)
            {
                case GlobalCommand.Show:
                    display();
                    break;
                case GlobalCommand.Hide:
                    minimize();
                    break;
                case GlobalCommand.Quit:
                    exit();
                    break;
            }
            return "";
        }
        private void pbMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            interact();
        }
        public void minimize()
        {
            Hide();
        }
        public void display()
        {
            //notifyIconMain.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
            Focus();
        }
        public void exit()
        {
            Application.Exit();
        }
        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimize();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            display();
        }

        private void notifyIconMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            display();
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit();
        }

        private void interactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            interact();
        }

        private void pbMain_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            dragStart = Control.MousePosition;
            windowLoc = Location;
        }

        private void pbMain_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void pbMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            int dx = 0, dy = 0;
            dx = Control.MousePosition.X - dragStart.X;
            dy = Control.MousePosition.Y - dragStart.Y;
            Location = new Point(windowLoc.X + dx, windowLoc.Y + dy);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D1:
                    moveToCorner(CornerIndex.LeftTop);
                    return true;
                case Keys.D1 | Keys.Shift:
                    changeGIF(0);
                    return true;
                case Keys.D2:
                    moveToCorner(CornerIndex.RightTop);
                    return true;
                case Keys.D2|Keys.Shift:
                    changeGIF(1);
                    return true;
                case Keys.D3: moveToCorner(CornerIndex.LeftBottom); return true;
                case Keys.D4: moveToCorner(CornerIndex.RightBottom); return true;
                case Keys.D5: moveToCorner(CornerIndex.LeftCentre); return true;
                case Keys.D3 | Keys.Shift:
                    changeGIF(2);
                    return true;
                case Keys.D4 | Keys.Shift:
                    changeGIF(3);
                    return true;
                case Keys.D5 | Keys.Shift:
                    changeGIF(4);
                    return true;
                case Keys.D6 | Keys.Shift:
                    changeGIF(5);
                    return true;
                case Keys.D7 | Keys.Shift:
                    changeGIF(6);
                    return true;
                case Keys.D8 | Keys.Shift:
                    changeGIF(7);
                    return true;
                case Keys.D9 | Keys.Shift:
                    changeGIF(8);
                    return true;
                case Keys.Left | Keys.Shift:
                    lastGIF();
                    return true;
                case Keys.Right | Keys.Shift:
                    nextGIF();
                    return true;
                case Keys.Enter: interact(); return true;
                default: break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        enum CornerIndex
        {
            LeftTop=0,
            RightTop,
            LeftBottom,
            RightBottom,
            LeftCentre,
        }
        private void moveToCorner(CornerIndex cidx)
        {
            switch (cidx)
            {
                case CornerIndex.LeftTop:
                    Location=new Point(0,0);
                    break;
                case CornerIndex.RightTop:
                    Location = new Point(m_nScrWidth-Width, 0);
                    break;
                case CornerIndex.LeftBottom:
                    Location = new Point(0, m_nScrHeight-Height);
                    break;
                case CornerIndex.RightBottom:
                    Location = new Point(m_nScrWidth - Width, m_nScrHeight - Height);
                    break;
                case CornerIndex.LeftCentre:
                    Location = new Point(m_nScrWidth - Width, (m_nScrHeight - Height) >> 1);
                    break;
            }
        }
        private void changeGIF(int fidx)
        {
            int tmp = currentGIFIdx;
            try
            {
                pbMain.Image = Image.FromFile(selectableGIFs[fidx]);
                currentGIFIdx = fidx;
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                currentGIFIdx = tmp;
            }
        }
        private void nextGIF()
        {
            currentGIFIdx++;
            if (currentGIFIdx > selectableGIFs.Count - 1) currentGIFIdx -= selectableGIFs.Count;
            changeGIF(currentGIFIdx);
        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            changeGIF(random.Next(0, selectableGIFs.Count));
            timerMain.Start();
        }

        private void FrmDisplay_Load(object sender, EventArgs e)
        {
        }

        private void lastGIF()
        {
            currentGIFIdx--;
            if (currentGIFIdx < 0) currentGIFIdx += selectableGIFs.Count;
            changeGIF(currentGIFIdx);
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
        public void initSelectableGIFs(string fname = "")
        {
            string ele;
            if (fname == "") fname = "gifs.ini";
            selectableGIFs = new List<string>();
            try
            {
                bool isFirst = true;
                foreach (string line in loadText(fname).Split('\n'))
                {
                    ele = line.Replace("\r", "");
                    if (isFirst)
                    {
                        try
                        {
                            swapGIFInterval = int.Parse(ele);
                        }
                        catch
                        {
                            swapGIFInterval = 0;
                        }
                        finally
                        {
                            isFirst = false;
                        }
                    }
                    else
                        selectableGIFs.Add(ele);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (swapGIFInterval > 0)
                {
                    timerMain.Enabled = true;
                    timerMain.Interval = swapGIFInterval * 1000;
                    timerMain.Start();
                }
                else
                {
                    timerMain.Enabled = false;
                }
            }
        }
    }
}

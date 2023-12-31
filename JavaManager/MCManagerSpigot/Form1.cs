using System.Diagnostics;
using System.Threading.Tasks;

namespace MCManager
{
    public partial class Form1 : Form
    {
        Process p = new Process();
        ProcessStartInfo pi = new ProcessStartInfo();
        List<string> Output = new List<string>();
        List<string> players = new List<string>();
        Task task;
        StreamWriter sw;
        bool ServerRunning = false;

        public Form1()
        {
            InitializeComponent();
        }

        private Action Start()
        {
            Dictionary<string, string> set = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader("settings.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains('='))
                    {
                        string[] tmp = line.Split('=');
                        set.Add(tmp[0], tmp[1]);
                    }
                }
            }

            pi.FileName = "start.cmd";
            string[] tmppath = set["jarfile"].Split('\\');
            pi.UseShellExecute = false;
            pi.RedirectStandardOutput = true;
            pi.RedirectStandardInput = true;
            pi.CreateNoWindow = true;
            p.StartInfo = pi;
            p.OutputDataReceived += new DataReceivedEventHandler(pOut);
            p.Start();
            sw = p.StandardInput;
            p.BeginOutputReadLine();
            ServerRunning = true;
            return null;
        }

        private void pOut(object sender, DataReceivedEventArgs e)
        {
            Output.Add(e.Data);
            Invoke((MethodInvoker)delegate { txtOut.Text += e.Data + Environment.NewLine; });
            if (ServerRunning && e.Data != null)
            {
                if (e.Data.Contains("Player connected"))
                {
                    string[] tmp = e.Data.Split(':');
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (tmp[i].Contains("Player connected"))
                        {
                            string[] tmpname = tmp[i + 1].Split(',');
                            if (!players.Contains(tmpname[0]))
                            {
                                players.Add(tmpname[0].Trim());
                                Invoke((MethodInvoker)delegate { cPlayers.Items.Add('"' + tmpname[0].Trim() + '"'); });
                                AutoCompleteStringCollection names = txtIn.AutoCompleteCustomSource;
                                Invoke((MethodInvoker)delegate { names.Add('"' + tmpname[0].Trim() + '"'); });
                            }
                        }
                    }
                }
            }
        }

        private void txtIn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    p.CancelOutputRead();
                }
                finally
                {
                    if (txtIn.Text == "gm1")
                    {
                        sw.WriteLine("gamemode 1 " + '"' + "Basti F6648" + '"');
                    }
                    else if (txtIn.Text == "gm0")
                    {
                        sw.WriteLine("gamemode 0 " + '"' + "Basti F6648" + '"');
                    }
                    else
                    {
                        sw.WriteLine(txtIn.Text);
                    }
                    p.BeginOutputReadLine();
                    txtIn.Text = "";
                    cPlayers.Text = "";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() => Start());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerRunning) btnstop.PerformClick();
        }

        private void btnstop_Click(object sender, EventArgs e)
        {
            try
            {
                p.CancelOutputRead();
            }
            finally
            {
                sw.WriteLine("stop");
                p.BeginOutputReadLine();
                p.CancelOutputRead();
                Thread.Sleep(1000);
                p.Close();
                StreamWriter log = new StreamWriter("log.txt");
                log.Write(txtOut.Text);
                log.Close();
                txtOut.Text = "Server stopped";
                ServerRunning = false;
            }
        }

        private void btnstart_Click(object sender, EventArgs e)
        {
            if (!ServerRunning)
            {
                txtOut.Text = "";
                p.Start();
                p.BeginOutputReadLine();
                ServerRunning = true;
            }
        }

        private void cPlayers_TextChanged(object sender, EventArgs e)
        {
            txtIn.Text += cPlayers.Text;
            cPlayers.Text = "";
            txtIn.Select();
        }
    }
}
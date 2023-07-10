using System.Diagnostics;
using System.Threading.Tasks;

namespace MCManager
{
    public partial class Form1 : Form
    {
        public Process p = new Process();
        ProcessStartInfo pi = new ProcessStartInfo();
        List<string> Output = new List<string>();
        List<string> players = new List<string>();
        Task task;
        StreamWriter sw;
        bool ServerRunning = false;
        bool ServerFound = false;
        Dictionary<string, string> set = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private Action Start()
        {
            string CurrentPath = "";
            string[] tmpsplit = System.Reflection.Assembly.GetEntryAssembly().Location.Split('\\');
            for (int i = 0; i < tmpsplit.Count() - 1; i++)
            {
                CurrentPath += tmpsplit[i] + '\\';
            }
            if (CurrentPath != "") Directory.SetCurrentDirectory(CurrentPath);


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
            if (!File.Exists(Directory.GetCurrentDirectory() + set["path"]))
            {
                MessageBox.Show("No server found. Please check the 'server' folder.");
                ServerFound = false;
                task = null;
                return null;
            }


            pi.FileName = Directory.GetCurrentDirectory() + set["path"];
            pi.UseShellExecute = false;
            pi.RedirectStandardOutput = true;
            pi.RedirectStandardInput = true;
            pi.CreateNoWindow = true;
            p.StartInfo = pi;
            p.OutputDataReceived += new DataReceivedEventHandler(pOut);
            p.Start();
            ServerFound = true;
            sw = p.StandardInput;
            p.BeginOutputReadLine();
            ServerRunning = true;
            return null;
        }

        private void pOut(object sender, DataReceivedEventArgs e)
        {
            try
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
                    if (e.Data.Contains("Player disconnected"))
                    {
                        string[] tmp = e.Data.Split(':');
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            if (tmp[i].Contains("Player disconnected"))
                            {
                                string[] tmpname = tmp[i + 1].Split(',');
                                if (!players.Contains(tmpname[0]))
                                {
                                    players.Remove(tmpname[0].Trim());
                                    Invoke((MethodInvoker)delegate { cPlayers.Items.Remove('"' + tmpname[0].Trim() + '"'); });
                                    AutoCompleteStringCollection names = txtIn.AutoCompleteCustomSource;
                                    Invoke((MethodInvoker)delegate { names.Remove('"' + tmpname[0].Trim() + '"'); });
                                }
                            }
                        }
                    }
                    if (e.Data.Contains("Port [19132] may be in use by another process"))
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            txtIn.Text = Environment.NewLine + Environment.NewLine +
                            "ERROR: Please restart your computer to free up the Port or change the Port in the server.properties." +
                            Environment.NewLine + Environment.NewLine;
                        });
                        txtIn.ReadOnly = true;
                    }
                }
            }
            catch
            {

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
                    switch (txtIn.Text)
                    {
                        case "gm1":
                            sw.WriteLine("gamemode 1 " + '"' + "Basti F6648" + '"');
                            break;
                        case "gm0":
                            sw.WriteLine("gamemode 0 " + '"' + "Basti F6648" + '"');
                            break;
                        case "gm sp":
                            sw.WriteLine("gamemode spectator " + '"' + "Basti F6648" + '"');
                            break;
                        default:
                            sw.WriteLine(txtIn.Text);
                            break;
                    }

                    p.BeginOutputReadLine();
                    if (txtIn.Text == null) btnstop.PerformClick();
                    txtIn.Text = "";
                    cPlayers.Text = "";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            task = Task.Factory.StartNew(() => Start());
            Thread.Sleep(4000);
            if (ServerFound == false)
            {
                int runnings = 0;
                while (runnings < 10)
                {
                    if (task == null)
                    {
                        this.Close();
                        Thread.Sleep(100);
                        return;
                    }
                    Thread.Sleep(100);
                    runnings++;
                }
                MessageBox.Show("An error occoured");
                this.Close();
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServerRunning) btnstop.PerformClick();
            task = null;
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
                Thread.Sleep(500);
                p.CancelOutputRead();
                Thread.Sleep(500);
                p.Close();
                StreamWriter log = new StreamWriter("log.txt");
                log.Write(txtOut.Text);
                log.Close();
                txtOut.Text = "Server stopped";
                ServerRunning = false;
                task = null;
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

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\server\\worlds");
            try
            {
                bool filesdone = false;
                foreach (string dir in dirs)
                {
                    if (!filesdone)
                    {
                        foreach (string file in Directory.GetFiles(dir))
                        {
                            string[] tmpfile = file.Split('\\');
                            string[] tmpdir = dir.Split('\\');

                            if (!Directory.Exists(set["BackupPath"] + '\\' + tmpdir[tmpdir.Length - 1] + '\\'))
                                Directory.CreateDirectory(set["BackupPath"] + '\\' + tmpdir[tmpdir.Length - 1] + '\\');

                            if (File.Exists(set["BackupPath"] + '\\' + tmpdir[tmpdir.Length - 1] + '\\' + tmpfile[tmpfile.Length - 1]))
                                File.Delete(set["BackupPath"] + '\\' + tmpdir[tmpdir.Length - 1] + '\\' + tmpfile[tmpfile.Length - 1]);
                            File.Copy(file, set["BackupPath"] + '\\' + tmpdir[tmpdir.Length - 1] + '\\' + tmpfile[tmpfile.Length - 1]);
                        }
                    }

                    string[] def = Directory.GetDirectories(dir);
                    foreach (string derdir in def)
                    {
                        foreach (string derdirfiles in Directory.GetFiles(derdir))
                        {
                            string[] tmpderdirfile = derdirfiles.Split('\\');
                            string[] tmpdbderdir = derdir.Split('\\');
                            
                            if (!Directory.Exists(set["BackupPath"] + '\\' + tmpdbderdir[tmpdbderdir.Length - 2] + '\\' + tmpdbderdir[tmpdbderdir.Length - 1] + '\\')) 
                                Directory.CreateDirectory(set["BackupPath"] + '\\' + tmpdbderdir[tmpdbderdir.Length - 2] + '\\' +tmpdbderdir[tmpdbderdir.Length - 1] + '\\');

                            if (File.Exists(set["BackupPath"] + '\\' + tmpdbderdir[tmpdbderdir.Length - 2] + '\\' + tmpdbderdir[tmpdbderdir.Length - 1] + '\\' + tmpderdirfile[tmpderdirfile.Length - 1]))
                                File.Delete(set["BackupPath"] + '\\' + tmpdbderdir[tmpdbderdir.Length - 2] + '\\' + tmpdbderdir[tmpdbderdir.Length - 1] + '\\' + tmpderdirfile[tmpderdirfile.Length - 1]);
                            File.Copy(derdirfiles, set["BackupPath"] + '\\' + tmpdbderdir[tmpdbderdir.Length - 2] + '\\' + tmpdbderdir[tmpdbderdir.Length - 1] + '\\' + tmpderdirfile[tmpderdirfile.Length - 1]);
                        }
                    }
                }

                
                MessageBox.Show("Backup was created");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error occoured while creating a backup");
            }
        }
    }
}
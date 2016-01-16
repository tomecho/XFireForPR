using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace XfireForPR
{
    public partial class Form1 : Form
    {
        string[] file;
        string path = "c:\\ProgramData\\Xfire\\";
        string conf = "xfire_games.ini";
        string backUpConf = "xfire_games.ini.bak";
        int? start = null;
        public static readonly string[] tehActualConf = { "[4578_3]", "LongName=Battlefield 2", "ShortName=bf2", @"LauncherDirKey=HKEY_LOCAL_MACHINE\SOFTWARE\Electronic Arts\EA GAMES\Battlefield 2\InstallDir", "LauncherExe=PRBF2.exe", "LauncherLoginArgs=+playerName %UA_GAME_LOGIN_NAME% +playerPassword %UA_GAME_LOGIN_PASSWORD%", "LauncherPasswordArgs=+password %UA_GAME_HOST_PASSWORD%", "LauncherNetworkArgs=+joinServer %UA_GAME_HOST_NAME% +port %UA_GAME_HOST_PORT% %UA_LAUNCHER_PASSWORD_ARGS%", "Launch=%UA_LAUNCHER_EXE_PATH% %UA_LAUNCHER_LOGIN_ARGS% +menu 1 +fullscreen 1 +restart 1 %UA_LAUNCHER_EXTRA_ARGS% %UA_LAUNCHER_NETWORK_ARGS%", "ServerStatusType=BF2", "ServerGameName=battlefield2", "ServerBroadcastPort=29900:51", "InGameRenderer=D3D9", "InGameFlags=DISABLE_RELEASE|BLOCK_ASYNC|ENABLE_MOUSE|NEW_INPUT_SYSTEM|USE_DINPUT_MOUSE|USE_PRESENT|USE_SWAPCHAIN|ENABLE_FPS_MODE", "ExcludeIPPorts=80,28910,29900,29901,29902,29903,29904", "GameClientDataType=BF2", "RunElevated=1" };
        public Form1()
        {
            InitializeComponent();
        }
        public void DoBackup()
        {
            if (File.Exists(path + backUpConf))
            {
                backUpConf = backUpConf + ".1"; //default second backup
                for (int i = 1; File.Exists(path + "xfire_games.ini.bak." + Convert.ToString(i)); i++)
                {
                    backUpConf = "xfire_games.ini.bak." + Convert.ToString((i+1));
                }
            }
            File.Move(path + conf, path + backUpConf);
        }
        public void WriteForced()
        {
            throw new NotImplementedException();
        }
        public void WriteNoForce()
        {
            StreamWriter sw = new StreamWriter(path + conf); //writes the new config           
            for (int i = 0; i < file.Length; i++)
            {
                if (i == start)
                {
                    sw.WriteLine();
                    sw.WriteLine();
                    for (int o = 0; o < tehActualConf.Length; o++)
                    {
                        sw.WriteLine(tehActualConf[o]);
                    }
                    continue;
                }
                sw.WriteLine(file[i]);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            StreamReader sr = new StreamReader(path + backUpConf);
            string line = null;
            int counter = 0;
            while ((line = sr.ReadLine()) != null)
            {
                file[counter] = line;
                counter++;
            }
            sr.Close();
            if (radioButton1.Checked)
            {
                WriteNoForce();
            }
            else if (radioButton2.Checked)
            {
                WriteForced();
            }
            else
            {
                MessageBox.Show("Select a radio button.");
                button1.Enabled = true;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(path + conf))
            {
                MessageBox.Show("Fatal Error missing xfire game config file!", "XfireForPR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            DoBackup();
            StreamReader sr = new StreamReader(path + backUpConf);
            string line = null;
            int counter = 0;
            bool inside = false;
            while ((line = sr.ReadLine()) != null) 
            {
                if (line == "[4578_2]") //last def in bf2 section
                {
                    inside = true;
                }
                if (inside == true && line == "RunElevated=1")
                {
                    start = counter;
                    inside = false;
                }
                counter++;
            }
            file = new string[counter];
            sr.Close();
            toolTip1.SetToolTip(this.radioButton1, "Must mannually select in xfire's game option menu.");
            toolTip1.SetToolTip(this.radioButton2, "Stops detection of vanilla bf2.");
        }
    }
}

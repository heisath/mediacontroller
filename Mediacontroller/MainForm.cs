using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using VolumeMixer_Lib;

namespace Mediacontroller
{
    public partial class MainForm : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void keybd_event(uint bVk, uint bScan, uint dwFlags, uint dwExtraInfo);


        string workDir = Environment.CurrentDirectory;
        ProcessStartInfo psi;
        VolumeMixer serialComms;
        Nixie nixie;

        public MainForm()
        {
            InitializeComponent();

            psi = new ProcessStartInfo(workDir + "\\nircmdc.exe")
            {
                CreateNoWindow = true,
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            serialComms = new VolumeMixer();
            serialComms.Start(tb_serialEVM.Text);
            serialComms.OnOutputChanged += (x) =>
            {
                switch (x)
                {
                    case 0:
                        notify_btnTonGemischt_Click(null, null);
                        break;
                    case 1:
                        notify_btnTonH600_Click(null, null);
                        break;
                    case 2:
                        notify_btnTonG935_Click(null, null);
                        break;
                }
            };
            serialComms.OnSoundboardPress += (x) =>
            {
                const uint VK_NUMPAD0 = 0x60, VK_DIVIDE = 0x6F, VK_MULTIPLY = 0x6A, VK_SUBTRACT = 0x6D, VK_ADD = 0x6B, VK_RETURN = 0x0D, KEYEVENTF_EXTENDEDKEY = 0x01, KEYEVENTF_KEYUP = 0x02;
                const uint VK_LCONTROL = 0xA2, VK_RCONTROL = 0xA3, VK_LMENU = 0xA4;


                keybd_event(VK_LCONTROL, 0, 0, 0);
                keybd_event(VK_RCONTROL, 0, KEYEVENTF_EXTENDEDKEY, 0);

                
                switch (x)
                {
                    case 10:
                        keybd_event(VK_DIVIDE, 0, KEYEVENTF_EXTENDEDKEY, 0); 
                        keybd_event(VK_DIVIDE, 0, KEYEVENTF_EXTENDEDKEY |KEYEVENTF_KEYUP, 0); break;
                    case 11:
                        keybd_event(VK_MULTIPLY, 0, KEYEVENTF_EXTENDEDKEY, 0); 
                        keybd_event(VK_MULTIPLY, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); break;
                    case 12:
                        keybd_event(VK_SUBTRACT, 0, KEYEVENTF_EXTENDEDKEY, 0); 
                        keybd_event(VK_SUBTRACT, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); break;
                    case 13:
                        keybd_event(VK_ADD, 0, KEYEVENTF_EXTENDEDKEY, 0); 
                        keybd_event(VK_ADD, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); break;
                    case 14:
                        keybd_event(VK_RETURN, 0, KEYEVENTF_EXTENDEDKEY, 0);
                        keybd_event(VK_RETURN, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); break;
                    default:
                        keybd_event((uint)(VK_NUMPAD0 + x), 0, 0, 0); 
                        keybd_event((uint)(VK_NUMPAD0 + x), 0, KEYEVENTF_KEYUP, 0);
                        break;
                }
                
              //  Thread.Sleep(100);
                keybd_event(VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);
                keybd_event(VK_RCONTROL, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

            };
        }


        private void btnConnectSerial_Click(object sender, EventArgs e)
        {
            nixie = new Nixie(txtComName.Text);
            if (nixie.serialPort.IsOpen)
            {
                panel1.Enabled = true;

                btnConnectSerial.Text = "Verbunden";
            }
            else
            {
                btnConnectSerial.Text = "FEHLER";
            }

        }
        private void btnSetNx_Click(object sender, EventArgs e)
        {
            int rest =
                Convert.ToByte(chkMisc7.Checked) << 7 |
                Convert.ToByte(chkMisc6.Checked) << 6 |
                Convert.ToByte(chkMisc5.Checked) << 5 |
                Convert.ToByte(chkMisc4.Checked) << 4 |
                Convert.ToByte(chkMisc3.Checked) << 3 |
                Convert.ToByte(chkMisc2.Checked) << 2 |
                Convert.ToByte(chkMisc1.Checked) << 1 |
                Convert.ToByte(chkMisc0.Checked);
            nixie.send((byte)nupNxStatus.Value, (byte)nupNx1.Value, (byte)nupNx2.Value, (byte)nupNx3.Value, (byte)nupNx4.Value, (byte)rest);
        }
        private void btnNxClock_Click(object sender, EventArgs e)
        {
            nixie.RunAsClock();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            nupNxStatus.Value = 3;
            nixie.send((byte)nupNxStatus.Value, (byte)nupNx1.Value, (byte)nupNx2.Value, (byte)nupNx3.Value, (byte)nupNx4.Value, 0);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            nupNxStatus.Value = 4;
            nixie.send((byte)nupNxStatus.Value, (byte)nupNx1.Value, (byte)nupNx2.Value, (byte)nupNx3.Value, (byte)nupNx4.Value, 0);
        }
        //---------------------------------------------------------------------


        private void ButtonHide_Click(System.Object sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Load(System.Object sender, System.EventArgs e)
        {
            StartUpTimer.Enabled = true;
        }

        private void StartUpTimer_Tick(System.Object sender, System.EventArgs e)
        {
            this.Hide();
            StartUpTimer.Enabled = false;
            btnConnectSerial_Click(sender, e);
            NixieStartUpTimer.Start();
        }

        private void NixieStartUpTimer_Tick(object sender, EventArgs e)
        {
            if (nixie.serialPort.IsOpen) btnNxClock_Click(sender, e);
            NixieStartUpTimer.Stop();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void notify_btnBeenden_Click(object sender, EventArgs e)
        {
            serialComms.Stop();
            this.Close();
        }

        private void notify_btnNixieAn_Click(object sender, EventArgs e)
        {
            nixie.RunAsClock();
        }

        private void notify_btnNixieAus_Click(object sender, EventArgs e)
        {
            nixie.send(0, 0, 0, 0, 0, 0);
        }


        private void notify_btnTonLautsprecher_Click(object sender, EventArgs e)
        {
            psi.Arguments = "setdefaultsounddevice \"Lautsprecher\" 1";
            Process.Start(psi);

            psi.Arguments = "setdefaultsounddevice \"Lautsprecher\" 2";
            Process.Start(psi);
        }

        private void notify_btnTonGemischt_Click(object sender, EventArgs e)
        {
            psi.Arguments = "setdefaultsounddevice \"Lautsprecher\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"H600 Headphones\" 2";
            Process.Start(psi);

            psi.Arguments = "setdefaultsounddevice \"H600 Mic\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"H600 Mic\" 2";
            Process.Start(psi);
        }

        private void notify_btnTonH600_Click(object sender, EventArgs e)
        {
            psi.Arguments = "setdefaultsounddevice \"H600 Headphones\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"H600 Headphones\" 2";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"H600 Mic\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"H600 Mic\" 2";
            Process.Start(psi);

        }

        private void notify_btnTonG935_Click(object sender, EventArgs e)
        {
            psi.Arguments = "setdefaultsounddevice \"G935 Headphones\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"G935 Headphones\" 2";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"G935 Mic\" 1";
            Process.Start(psi);
            psi.Arguments = "setdefaultsounddevice \"G935 Mic\" 2";
            Process.Start(psi);

        }

        private void btn_restartEVM_Click(object sender, EventArgs e)
        {
            serialComms.Stop();
            serialComms.Start(tb_serialEVM.Text);
        }
    }
}

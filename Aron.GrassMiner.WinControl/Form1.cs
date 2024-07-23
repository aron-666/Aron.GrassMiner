using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Aron.GrassMiner.WinControl
{
    public partial class Form1 : Form
    {
        private const int ERROR_INSUFFICIENT_BUFFER = 122;
        private const int AF_INET = 2;
        private const int MIB_TCP_STATE_ESTAB = 5;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        static extern int GetExtendedTcpTable(byte[] buffer, ref int dwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, int reserved = 0);
        [StructLayout(LayoutKind.Sequential)]
        private struct MIB_TCPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            public MIB_TCPROW_OWNER_PID table;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MIB_TCPROW_OWNER_PID
        {
            public int state;
            public int localAddr;
            public int localPort;
            public int remoteAddr;
            public int remotePort;
            public int owningPid;
        }

        private enum TCP_TABLE_CLASS
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL

        }



        private static List<int> GetTcpConnections(int pid)
        {
            int bufferSize = 0;
            int result = GetExtendedTcpTable(null, ref bufferSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_MODULE_ALL, 0);
            if (result != ERROR_INSUFFICIENT_BUFFER)
            {
                throw new InvalidOperationException("Failed to get table size.");
            }

            byte[] buffer = new byte[bufferSize];
            result = GetExtendedTcpTable(buffer, ref bufferSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_MODULE_ALL, 0);
            if (result != 0)
            {
                throw new InvalidOperationException("Failed to get TCP table.");
            }

            IntPtr tablePtr = Marshal.AllocHGlobal(bufferSize);
            try
            {
                Marshal.Copy(buffer, 0, tablePtr, bufferSize);
                var table = (MIB_TCPTABLE_OWNER_PID)Marshal.PtrToStructure(tablePtr, typeof(MIB_TCPTABLE_OWNER_PID));
                int rowSize = Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID));


                List<int> ports = new List<int>();

                for (int i = 0; i < table.dwNumEntries; i++)
                {
                    var rowPtr = (IntPtr)((long)tablePtr + Marshal.SizeOf(typeof(uint)) + (i * rowSize));
                    var row = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(rowPtr, typeof(MIB_TCPROW_OWNER_PID));

                    if (row.owningPid == pid)
                    {
                        ports.Add(IPAddress.NetworkToHostOrder((short)row.localPort));
                        Console.WriteLine($"Local Address: {IPAddress.NetworkToHostOrder(row.localAddr)}:{IPAddress.NetworkToHostOrder((short)row.localPort)}, " +
                                          $"Remote Address: {IPAddress.NetworkToHostOrder(row.remoteAddr)}:{IPAddress.NetworkToHostOrder((short)row.remotePort)}, " +
                                          $"State: {row.state}");
                    }
                }
                return ports;
            }
            finally
            {
                Marshal.FreeHGlobal(tablePtr);
            }
            return null;
        }
        string appsetting = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
        string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
        string exe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app", "Aron.GrassMiner.exe");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var temp = new TodoCheckBox();
            //temp.Name = ckServiceInstalled.Name;
            //temp.Text = ckServiceInstalled.Text;
            //temp.Checked = ckServiceInstalled.Checked;
            //temp.Location = ckServiceInstalled.Location;
            //temp.Size = ckServiceInstalled.Size;
            //temp.Font = ckServiceInstalled.Font;
            //temp.AutoSize = ckServiceInstalled.AutoSize;
            //temp.CheckedChanged += (s, e) =>
            //{
            //    ckServiceInstalled.Checked = temp.Checked;
            //};
            //groupBox1.Controls.Add(temp);
            //groupBox1.Controls.Remove(ckServiceInstalled);
            //ckServiceInstalled.Dispose();
            //ckServiceInstalled = temp;
            //ckServiceInstalled.BringToFront();
            //groupBox1.ResumeLayout(false);
            //groupBox1.PerformLayout();


        }

        private void ckServiceInstalled_CheckedChanged(object sender, EventArgs e)
        {
            if (ckServiceInstalled.Checked)
            {
                ckServiceInstalled.BackColor = Color.Blue;
            }
            else
            {
                ckServiceInstalled.BackColor = Color.White;
            }
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            
            //檢查是否正在執行
            var prc = Process.GetProcesses()
                .FirstOrDefault(p =>
                {
                    try
                    {
                        return string.Equals(p.MainModule.FileName, exe, StringComparison.OrdinalIgnoreCase);

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });
            if (prc != null)
            {
                // 傳送中止指令
                prc.Kill();
                return;
            }


            //run dotnet app\Aron.GrassMiner.dll
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = app;
            startInfo.FileName = exe;
            startInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = appsetting;

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;

            process.Start();

        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            //open file explorer to app\\logs

            string app = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
            string logs = Path.Combine(app, "logs");
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "explorer.exe";
            startInfo.Arguments = logs;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //檢查是否正在執行
            var prc = Process.GetProcesses()
                .FirstOrDefault(p =>
                {
                    try
                    {
                        return string.Equals(p.MainModule.FileName, exe, StringComparison.OrdinalIgnoreCase);

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });

            if (prc != null)
            {
                btnControl.Text = "Stop";
                lbModule.BackColor = Color.Green;
                lbPid.Text = prc.Id.ToString();
                var xxx = GetTcpConnections(prc.Id);
                if(xxx.Count > 0)
                {
                    lbPorts.Text = xxx.Select(x => x.ToString()).Aggregate((x, y) =>
                    string.IsNullOrEmpty(y) ? x ?? "" : $"{x}, {y}");
                }
                else
                {
                    lbPorts.Text = "";
                }
            }
            else
            {
                btnControl.Text = "Start";
                lbModule.BackColor = Color.Transparent;
                lbPid.Text = "";
                lbPorts.Text = "";
            }


        }
    }
}


public class TodoCheckBox : CheckBox
{
    public override bool AutoSize
    {
        get => base.AutoSize;
        set => base.AutoSize = false;
    }

    public TodoCheckBox()
    {
        this.TextAlign = ContentAlignment.MiddleRight;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        int h = this.ClientSize.Height - 2;
        var rc = new Rectangle(new Point(-1, this.Height / 2 - h / 2), new Size(h, h));
        if (this.FlatStyle == FlatStyle.Flat)
        {
            ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Flat | ButtonState.Checked : ButtonState.Flat | ButtonState.Normal);
        }
        else
        {
            ControlPaint.DrawCheckBox(e.Graphics, rc, this.Checked ? ButtonState.Checked : ButtonState.Normal);
        }
    }
}
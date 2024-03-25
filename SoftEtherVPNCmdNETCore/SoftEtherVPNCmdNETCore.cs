using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SoftEtherVPNCmdNETCore
{
    public class cSoftEtherVPNCmdNETCore
    {
        public string Binary { get;  set; }
        public int CommandTimeoutSec { get; set; } = 30;

        /// <summary>
        /// Initialize the library by setting the @binary to the path on the system
        /// </summary>
        /// <param name="binary">The binary path</param>
        public cSoftEtherVPNCmdNETCore(string binary)
        {
            this.Binary = binary;
        }

        /// <summary>
        /// Initialize the library with default vpncmd as binary
        /// </summary>
        public cSoftEtherVPNCmdNETCore()
        {
            this.Binary = "vpncmd";
        }

        /// <summary>
        /// Execute a SoftEther vpncmd command
        /// </summary>
        /// <param name="hostPort">Host:Port where you connect</param>
        /// <param name="type">CLIENT, SERVER or TOOLS</param>
        /// <param name="command">Sofether command</param>
        /// <param name="parameters">Parameters for the command</param>
        /// <param name="extraArguments">Extra options needed for example /PASSWORD or /HUB</param>
        /// <returns>Return CSV output of the result of the command</returns>
        public async Task<string> ExecuteCommand(string hostPort, string type, string command, string parameters, string? password = null, string extraArguments = "")
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = this.Binary,
                    Arguments = hostPort + $" /PASSWORD:{password}" + " /" + type + " /CSV " + extraArguments + " /CMD " + command + " " + parameters ,
                    //RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            //string[] parms = parameters.Split(' ');
            //foreach (var c in parms)
            //{
            //    process.StandardInput.WriteLine(c);
            //}

            //process.StandardInput.Close();

            Task<string> readOutputTask = process.StandardOutput.ReadToEndAsync();
            Task<string> readErrorTask = process.StandardError.ReadToEndAsync();
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(CommandTimeoutSec));
            Task readTask = Task.WhenAll(readErrorTask, readErrorTask);
            Task completedTask = await Task.WhenAny(readTask, timeoutTask);

            string output = "";
            string error = "";
            if (completedTask == readTask)
            {
                output = readOutputTask.Result;
                error = readErrorTask.Result;
                process.Kill();

            }
            else
            {
                readOutputTask.Dispose();
                readErrorTask.Dispose();

                process.Kill();
                throw new Exception("Timeout");
            }
            
            process.Dispose();

            if (!String.IsNullOrEmpty(error))
                throw new Exception(error);

            //Exclude the Connected to IP line and newline character
            return output.Substring(output.IndexOf('\n') + 2); 
        }

        /// <summary>
        /// Function parses CSV format from SoftEther
        /// </summary>
        /// <param name="output">Output from ExecuteCommand</param>
        /// <returns>KeyValue pairs in dictionary that are easy to read</returns>
        public Dictionary<string,string> ParseCommand(string output)
        {
            var lines = output.Split('\n');
            var dict = new Dictionary<string, string>();
            for(int i = 0; i < lines.Length; i++)
            {
                if (String.IsNullOrEmpty(lines[i]))
                    continue;

                var split = lines[i].Split(',');

                if (i == 0 && split[0] != "Item" && split[1] != "Value")
                    throw new Exception("Invalid vpncmd output, no Item,Value!");

                if(i != 0)
                    dict.Add(split[0], split[1]);
            }
            return dict;
        }

        /// <summary>
        /// Execute and Parse SoftEther vpncmd command
        /// </summary>
        /// <param name="hostPort">Host:Port where you connect</param>
        /// <param name="type">CLIENT, SERVER or TOOLS</param>
        /// <param name="command">Sofether command</param>
        /// <param name="parameters">Parameters for the command</param>
        /// <param name="extraArguments">Extra options needed for example /PASSWORD or /HUB</param>
        /// <returns>Return CSV output of the result of the command</returns>
        public async Task<Dictionary<string, string>> ExecuteCommandAndParse(string hostPort, string type, string command, string parameters, string? password = null, string extraArguments = "")
        {
            return ParseCommand(await ExecuteCommand(hostPort, type, command, parameters, password, extraArguments));
        }
    }
}

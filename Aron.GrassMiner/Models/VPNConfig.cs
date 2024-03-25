using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aron.GrassMiner.Models
{
    public class VPNConfig
    {
        public string Config { get; set; }
        public string Location { get; set; }
        public int Ping { get; set; }
        public string Protocol { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public int Speed { get; set; }
    }
}

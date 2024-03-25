using System;
using System.Collections.Generic;
using System.Text;

namespace SoftEtherVPNCmdNETCore.VPNServer
{
    public class cServerCipher
    {
        public string selectedEncryptedAlgorithm { get; set; }
        public List<string> usableEncryptedAlgorithmNames { get; set; }
    }
}

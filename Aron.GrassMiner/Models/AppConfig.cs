﻿namespace Aron.GrassMiner.Models
{
    public class AppConfig
    {
        //public bool IsCommunity { get; set; } = false;
        public string UserName { get; set; }
        public string Password { get; set; }

        public string? AdminUserName { get; set; }

        public string? AdminPassword { get; set; }

        public bool ShowChrome { get; set; }

        public string? ProxyEnable { get; set; } = "false";

        public string? ProxyHost { get; set; }

        public string? ProxyUser { get; set; }
        public string? ProxyPass { get; set; }

        public bool LogEnable { get; set; } = false;
    }
}

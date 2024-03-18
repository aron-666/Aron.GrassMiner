﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace GrassMiner.Models
{
    public class MinerRecord
    {
        public string? AppVersion { get; set; } = null;
        /// <summary>
        /// 連線使用者名稱
        /// </summary>
        [Display(Name = "使用者名稱")]
        public string? LoginUserName { get; set; }

        /// <summary>
        /// 最後的錯誤訊息
        /// </summary>
        [Display(Name = "錯誤訊息")]
        public string? Exception { get; set; }


        /// <summary>
        /// 錯誤發生時間 
        /// </summary>
        [Display(Name = "錯誤時間")]
        public DateTime? ExceptionTime { get; set; }


        /// <summary>
        /// 是否連線
        /// </summary>
        [Display(Name = "是否連線")]
        public bool IsConnected { get; set; } = false;


        /// <summary>
        /// 重連秒數
        /// </summary>
        [Display(Name = "重連秒數")]
        public int ReconnectSeconds { get; set; } = 0;


        [Display(Name = "重連次數")]
        public int ReconnectCounts { get; set; } = 0;


        /// <summary>
        /// 狀態
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))] 
        public MinerStatus Status { get; set; }

        /// <summary>
        /// 外網IP
        /// </summary>
        [Display(Name = "外網IP")]
        public string PublicIp { get; set; } = "";

        /// <summary>
        /// 總共挖到的點數
        /// </summary>
        [Display(Name = "點數")]
        public string Points { get; set; }

    }
}

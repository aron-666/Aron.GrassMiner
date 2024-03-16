using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace GrassMiner.Models
{
    public class MinerRecord
    {
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

    }

    public enum MinerStatus
    {
        [Display(Name = "挖礦中，不要吵我")]
        Connected,
        [Display(Name = "幹，斷線了")]
        Disconnected,
        [Display(Name = "應用程式啟動中")]
        AppStart,
        [Display(Name = "登入中")]
        LoginPage,
        [Display(Name = "你他媽帳號密碼打錯了")]
        LoginError,
        [Display(Name = "程式掛了，請回報錯誤訊息")]
        Error,
        [Display(Name = "程式停止中")]
        Stop
    }
}

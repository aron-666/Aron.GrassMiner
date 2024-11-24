using System.ComponentModel.DataAnnotations;

namespace Aron.GrassMiner.Models
{

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

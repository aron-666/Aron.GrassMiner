namespace Aron.GrassMiner.ViewModels
{
    /// <summary>
    /// 登入回傳
    /// </summary>
    public class LoginResp
    {
        /// <summary>
        /// 憑證類型
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 憑證
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 時效(秒)
        /// </summary>
        public int expires_in { get; set; }

        //public string refresh_token { get; set; }


        public string email { get; set; }
        public string username { get; set; }

        public IEnumerable<string> roles { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Newtonsoft.Json.JsonProperty(".issued")]
        public DateTime issued { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        [Newtonsoft.Json.JsonProperty(".expires")]
        public DateTime expires { get; set; }
    }
}

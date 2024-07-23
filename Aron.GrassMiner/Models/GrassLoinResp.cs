namespace Aron.GrassMiner.Models
{

    public class GrassLoinResp
    {
        public Result result { get; set; }
    }

    public class Result
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string userId { get; set; }
        public string email { get; set; }
        public string userRole { get; set; }
    }

}

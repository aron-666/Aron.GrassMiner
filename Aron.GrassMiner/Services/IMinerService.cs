using OpenQA.Selenium.Chrome;

namespace GrassMiner.Services
{
    public interface IMinerService
    {
        public ChromeDriver driver { get; set; }

        void Start();
        void Stop();
    }
}
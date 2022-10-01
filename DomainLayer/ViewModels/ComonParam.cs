

namespace DomainLayer.ViewModels
{
    public class ComonParam /*for pagination and filteration*/
    {
        public int first { get; set; }
        public int page { get; set; }
        public int pageCount { get; set; }
        public int rows { get; set; }
        public string gloabalText { get; set; } = ""; /*title or manager name*/
        public string userId { get; set; }
    }
}

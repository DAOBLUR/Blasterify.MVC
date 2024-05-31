namespace Blasterify.Models.Yuno.Payment
{
    public class BrowserInfo
    {
        public string User_Agent { get; set; }
        public string Accept_Header { get; set; }
        public object Accept_Content { get; set; }
        public object Accept_Browser { get; set; }
        public string Color_Depth { get; set; }
        public string Screen_Height { get; set; }
        public string Screen_Width { get; set; }
        public object Javascript_Enabled { get; set; }
        public object Java_Enabled { get; set; }
        public object Browser_Time_Difference { get; set; }
        public string Language { get; set; }
    }
}
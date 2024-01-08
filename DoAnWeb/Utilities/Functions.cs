using System.Security.Cryptography;
using System.Text;

namespace DoAnWeb.Utilities
{
    public class Functions
    {
        public static int _AccountId = 0;
        public static string _UserName = string.Empty;
        public static string _FullName = string.Empty;
        public static int _Phone = 0;
        public static string _Email = string.Empty;
        public static string _Message = string.Empty;
        public static string _MessageEmail = string.Empty;


        public static string TitleSlugGeneration(string type, string title, int id)
        {
            string sTitle = type + "-" + SlugGenerator.SlugGenerator.GenerateSlug(title) + "-" + id.ToString() + ".html";
            return sTitle;
        }

        public static string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static bool IsLogin()
        {
            if (string.IsNullOrEmpty(Functions._UserName) || string.IsNullOrEmpty(Functions._Email) || (Functions._AccountId <= 0))
                return false;
            return true;
        }

    }
}

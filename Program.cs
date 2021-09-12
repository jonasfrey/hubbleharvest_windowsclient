using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

// using Microsoft.Win32;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Runtime.InteropServices;
// using System.Text;
// using System.Threading.Tasks;
// using System.Timers;

// using Microsoft.Win32;
// using System.Drawing;
// using System.Drawing.Imaging;
// using System.IO;
// using System.Runtime.InteropServices;

using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.IO;


namespace backgroundchanger
{
    class Program
    {

        // [DllImport("user32.dll", CharSet = CharSet.Auto)]
        // static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        // private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        // private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        // private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(
        UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);
        private static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        private static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        private static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        static void Main(string[] args)
        {

            string downloaded_images_folder_name = "downloaded_images";

            DateTime date_time = DateTime.Now;

            long unix_timestamp = ((DateTimeOffset)date_time).ToUnixTimeSeconds();

            // using (WebClient client = new WebClient()) 
            // {
            //     // OR 
            //     //client.DownloadFileAsync(new Uri("https://hubbleharvest.ch:8080"), @"c:\temp\image35.jpg");
            // }
            WebClient client = new WebClient();
            string img_path = @$".\{downloaded_images_folder_name}\{unix_timestamp}.jpg";
            //client.DownloadFileAsync(new Uri("http://hubbleharvest.ch:8080"), img_path);
            client.DownloadFile(new Uri("http://hubbleharvest.ch:8080"), img_path);

            long filesize = new System.IO.FileInfo(img_path).Length;
            
            Console.WriteLine(filesize);

            List<string> str_list = new List<string>();
            str_list.Add("wtf");
            str_list.Add("why");
            str_list.Add("would");
            str_list.Add("you");
            str_list.Add("do");
            str_list.Add("that");
            Console.WriteLine("Hello World!");

            foreach(var str in str_list)
            {
                Console.WriteLine(str);
            }

            SetWallpaper(img_path);

        }


        static public void SetWallpaper(String path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", 0.ToString()); // 2 is stretched
            key.SetValue(@"TileWallpaper", 0.ToString());
 
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        
    }

    
}


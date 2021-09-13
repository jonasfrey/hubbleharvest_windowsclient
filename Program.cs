using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;

using System.Collections; 

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
using System.Collections;
using System.Linq;



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

                        
            Console_canvas cc = new Console_canvas(10, 10);


            string rendered_string = cc.render();
            
            Console.WriteLine(rendered_string);

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

            string img_path_absolute = Path.GetFullPath(img_path);

            //client.DownloadFileAsync(new Uri("http://hubbleharvest.ch:8080"), img_path);
            client.DownloadFile(new Uri("http://hubbleharvest.ch:8080"), img_path_absolute);

            long filesize = new System.IO.FileInfo(img_path).Length;
            
            Console.WriteLine(filesize);

            SetWallpaper(img_path_absolute);


        }


        static public void SetWallpaper(String path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            
            key.SetValue(@"WallpaperStyle", 0.ToString()); // 2 is stretched
            key.SetValue(@"TileWallpaper", 0.ToString());
 
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

    

        
    }

    class Console_canvas{

        public int width = 20; 
        public int height = 20; 

        public string[][] string_array_2d = null; 

        public char empty_char = ' '; 

        public XY_object[] xy_objects = {};

        public string output = "";
        
        
        public Console_canvas(int passed_width, int passed_height){
            width = passed_width;
            height = passed_width;

            //string_array_2d = Enumerable.Repeat(true, height).ToArray();
        }
        public char[][] get_empty_string_array_2d(){

            string column_string = new string(empty_char, width);
            char[] column_string_array = column_string.ToCharArray();

            char[][] row_string_array = Enumerable.Repeat(column_string_array, height).ToArray();

            return row_string_array; 

            // for(int row = 0 ; row < height; row++){
            //     string[] row_array = new string[width];
            //     for(int column = 0; column < width; column++){
            //         row_array[row] = empty_char;
            //     }   
            // }
        }

        public string render(){

            //todo foreach xy_object replace row_string_array[x][y] value with  the xy_object.char 
            // foreach (var xy_object in xy_objects)
            // {

            // }

            char[][] row_string_array = get_empty_string_array_2d();    

            string[] lines = {};

            string final_string = ""; 
            for(int i = 0; i < row_string_array.Length; i++){
                string row_string = new string(row_string_array[i]);

                //lines[i] = new string(row_string_array[i]);
                lines.Append(row_string);

                final_string += row_string + "\n";

                // foreach(var chararray in row_string_array){
                //     lines.Append(new string(chararray));
                // }
            }

            string final_output_string = string.Join("\n", lines);
            
            return final_string; 
        }

    }

    class XY_object{
        public int x = 0; 
        public int y = 0; 
        public string character = "#";

        public XY_object(int x_passed, int y_passed){
            
            x = x_passed; //disadvantage passed arguments have to have a different variable name 
            y = y_passed; 


        }

    }

}


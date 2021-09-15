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

                        
            Console_canvas cc = new Console_canvas(50, 30);
            XY_object xy_object = new XY_object(2,2);
            
            // xy_object.ren_fun test = new xy_object.ren_fun(
            //     (x) => {return "asdf" };
            // );

            xy_object.re_fu = (xy_object) => {
                // do
                xy_object.velocity_y = 0;
                xy_object.velocity_x = 0; 

                if (Console.KeyAvailable)  
                {  
                    ConsoleKeyInfo console_key_info =  Console.ReadKey(true);
                    

                    if(console_key_info.Key == ConsoleKey.UpArrow){
                        xy_object.velocity_y = -1;
                        xy_object.velocity_x = 0; 
                    }
                    if(console_key_info.Key == ConsoleKey.DownArrow){
                        xy_object.velocity_y = 1;
                        xy_object.velocity_x = 0; 
                    }
                    if(console_key_info.Key == ConsoleKey.LeftArrow){
                        xy_object.velocity_y = 0;
                        xy_object.velocity_x = -1; 
                    }
                    if(console_key_info.Key == ConsoleKey.RightArrow){
                        xy_object.velocity_y = 0;
                        xy_object.velocity_x = 1; 
                    }

                }  

                xy_object.position_x = xy_object.position_x + xy_object.velocity_x;
                xy_object.position_y = xy_object.position_y + xy_object.velocity_y;

            };
            
            cc.xy_objects.Add(xy_object);


            cc.start_loop_repeat_render();

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


        static public void SetWallpaper(String path, string style = "centered")
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            
            key.SetValue(@"WallpaperStyle", 0.ToString()); // 2 is stretched
            key.SetValue(@"TileWallpaper", 0.ToString());

            if (style == "stretched")
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == "centered")
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }

            if (style == "tiled")
            {
                key.SetValue(@"WallpaperStyle", 1.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }


            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

    

        
    }

    class Console_canvas{

        public int width = 50; 
        public int height = 50; 

        public bool loop_repeat_render_running = false; 
        public string[][] string_array_2d = null; 

        public char empty_char = '.'; 

        public List<XY_object> xy_objects = new List<XY_object>();

        public int fps = 30; 

        public string output = "";
        
        
        public Console_canvas(int passed_width, int passed_height){
            width = passed_width;
            height = passed_height;

            //string_array_2d = Enumerable.Repeat(true, height).ToArray();
        }
        public char[][] get_empty_string_array_2d(){

            //string column_string = new string(empty_char, width);
            //char[] column_string_array = column_string.ToCharArray();
            //if using column_string_array, then copy it without reference

            char[][] row_string_array = Enumerable.Range(1, height).Select(i =>  new string(empty_char, width).ToCharArray()).ToArray();
            //Using Enumerable.Repeat this way will initialize only one object and return that object every time when you iterate over the result. so the object will be a reference
            //char[][] row_string_array = Enumerable.Repeat(new string(empty_char, width).ToCharArray(), height).ToArray();

            return row_string_array; 

            // for(int row = 0 ; row < height; row++){
            //     string[] row_array = new string[width];
            //     for(int column = 0; column < width; column++){
            //         row_array[row] = empty_char;
            //     }   
            // }
        }

        public string get_rendered_string(){

            char[][] row_string_array = get_empty_string_array_2d();

            //todo foreach xy_object replace row_string_array[x][y] value with  the xy_object.char 

            foreach(var xy_object in xy_objects)
            {
                if(xy_object.position_y > 0 && xy_object.position_y < height){
                    if(xy_object.position_x > 0 && xy_object.position_x < width){
                        row_string_array[xy_object.position_y][xy_object.position_x] = xy_object.character;
                    }
                }

            }


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

        public void start_loop_repeat_render(){
            loop_repeat_render_running = true; 
            loop_repeat_render();
        }

        public void stop_loop_repeat_render(){
            loop_repeat_render_running = false; 
        }
        public void clear(){


            //string new_lines_for_console_clearing = new string('\n', height);

            //Console.WriteLine(new_lines_for_console_clearing);

            Console.Clear();

        }
        public void loop_repeat_render(){
            
            if(loop_repeat_render_running){
                if (Console.KeyAvailable)  
                {  
                    ConsoleKeyInfo console_key_info =  Console.ReadKey(true);
                    
                    if(console_key_info.Key == ConsoleKey.Q){
                        loop_repeat_render_running = false; 
                        Console.WriteLine("Game quit q!");  
                    }  

                }  


                foreach(var xy_object in xy_objects)
                {
                    xy_object.re_fu(xy_object);
                }


                string rendered_string = get_rendered_string();
                
                clear();

                Console.WriteLine("keep <^> arrow key pressed to move");

                Console.WriteLine(rendered_string);


                System.Threading.Thread.Sleep((int) (1000/fps));


                loop_repeat_render();
            }

        }

    }

    class XY_object{ 

 
        // public Delegate render_function;
        // delegate void TestDelegate(string s);
        //public Func<string, string> rend_func;
        //public delegate void ren_fun(); 

        public Action<XY_object> re_fu; 

        //public delegate void Action<XY_object> r_f;

        public int position_x = 0; 
        public int position_y = 0; 

        public int velocity_x = 0; 
        public int velocity_y = 0; 

        public int acceleration_x = 0; 
        public int acceleration_y = 0; 


        public char character = '#';

        public XY_object(int position_x_passed, int position_y_passed){
            
            position_x = position_x_passed; //disadvantage passed arguments have to have a different variable name 
            position_y = position_y_passed; 


        }

    }

}


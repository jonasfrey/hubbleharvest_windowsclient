using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
namespace backgroundchanger
{
    class Program
    {
        static void Main(string[] args)
        {

            string downloaded_images_folder_name = "downloaded_images";

            DateTime date_time = DateTime.Now;
            long unix_timestamp = ((DateTimeOffset)date_time).ToUnixTimeSeconds();

            using (WebClient client = new WebClient()) 
            {
                client.DownloadFile(new Uri("http://hubbleharvest.ch:8080"), @$".\{downloaded_images_folder_name}\{unix_timestamp}.jpg");
                // OR 
                //client.DownloadFileAsync(new Uri("https://hubbleharvest.ch:8080"), @"c:\temp\image35.jpg");
            }

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
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using System.IO;
namespace Otomasyon_LeftSoft
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            var task = Task.Run((Func<Task>)Program.Run);
            task.Wait();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            */
            Application.Run(new Form1());
        }



        static string token = "H5rbKWBjFWAAAAAAAAAASWo8Ap5wGztu3C2Js6Zqe-EwnjYpwlHtyA7E0uBlqrjT";
        static async Task Run()
        {
            using (var dbx = new DropboxClient(token))
            {
                string folder = "";
                string file = "appUpdate.xml";
                using(var response = await dbx.Files.DownloadAsync(folder+"/"+file))
                {
                    var d = response.GetContentAsByteArrayAsync();
                    d.Wait();
                    var s = d.Result;
                    File.WriteAllBytes(file, s);
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace Ali.Analyze
{
    public class Program
    {
        private static bool isItemLine;
        private static Regex ipAddressRegEx = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

        [STAThread]
        public static void Main(string[] args)
        {
            Console.Title = @"Anaalyze  various IP Addresses";
            Console.Write(@"Hello");
            Console.Write("\nDo you want to start?\n Press Y to continue or N to Exit ");
            var response = Convert.ToChar(Console.ReadLine());
            if (response.ToString().ToLower() != "y") return;
            Console.Write("Upload the desired log file");
            System.Threading.Thread.Sleep(500);
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open csv Document"
            };
            using (dialog)
            {
                var index = 0;
                //In the debug mode, I am displaying the first 5 items in order to ensure that the `regEX.Match.Success` is working correctly, and to actually be able to view the outputed result.
#if DEBUG
                var maxVal = 5;
#else
                var maxVal = Int32.MaxValue;
#endif
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(File.OpenRead(dialog.FileName));
                    var lines = ReadLine(reader);
                    //while (!reader.endofstream && index <= maxval)
                    //{
                    //    var line = reader.readline();
                    //    //var lines = reader.readtoend().replace("-", " ").split(new string[] { "\r\n" }, stringsplitoptions.none).select(s => s.split('\t'));
                        
                    //    if (line == null) continue;
                    //    console.writeline(evalline(line));
                    //    index++;
                    //}
                }
            }
            Console.WriteLine(@"Finished.");
            Console.ReadKey(false);
        }
        private static IEnumerable<string[]> ReadLine(TextReader tr)
        {
            using (tr)
                for (string line = tr.ReadLine(); line != null; line = tr.ReadLine())
                    yield return line.Split('\t');
        }


        private static string EvalLine(string line)
        {
            if (!isItemLine && !ipAddressRegEx.Match(line).Success)
                return @"Not a valid line.";
            line = line.Replace("-", "");
            var cols = line.Split(' ');
            return line;
               
        }
    }
}

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace Ali.Analyze
{
    public class Analyze
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var ipAddressRegEx = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            var responseRegEx = new Regex(@"- (\d{3}) .+ (\d{3}) \d+ HTTP");            
            Console.Title = @"Anaalyze  various IP Addresses";
            Console.Write(@"Hello");
            Console.Write("\nDo you want to start?\n Press Y to continue or N to Exit ");
            var response = Convert.ToChar(Console.ReadLine());
            if (response.ToString().ToLower() != "y") return;
            Console.WriteLine("Upload the desired log file");
            System.Threading.Thread.Sleep(500);
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Open csv Document"
            };
            var result = Enumerable.Empty<Result>();
            var lines = Enumerable.Empty<string>();
            var addresses = Enumerable.Empty<Address>();
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var reader = new StreamReader(File.OpenRead(dialog.FileName));
                    lines = reader.ReadToEnd()
                        .Split(new string[] { "\r\n" }, StringSplitOptions.None)
                        .Where(el => ipAddressRegEx.Match(el).Success);

                    addresses = (from el in lines
                                     let ipMatch = ipAddressRegEx.Matches(el)
                                     let responseMatch = responseRegEx.Match(el)
                                     select new Address
                                     {
                                         RequestIpAddress = ipMatch[0].Value,
                                         IpAddress = ipMatch[1].Value,                                         
                                         Status = responseMatch.Groups[1].Value
                                     })
                                    .Where(a => a.Status == "200" && !a.RequestIpAddress.StartsWith("207.114"));
                    result = addresses
                        .GroupBy(a => a.RequestIpAddress)
                        .Select(a => new Result
                        {
                            IpAddress = a.First().RequestIpAddress,
                            Quantity = a.Count()
                        })
                        .OrderByDescending(a => a.Quantity);
#if DEBUG
                    foreach(var item in result)
                    {
                        Console.WriteLine($"{item.Quantity}, IP Address: {item.IpAddress}");
                    }
#endif
                }
            }
            var csv = Encoding.Default.GetBytes(result.ToCsv());
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Report.csv");
            Console.WriteLine($"Out of {lines.Count()} valid lines, {addresses.Count()} of them were evaluated due to pre-set conditions.");
            Console.WriteLine($"Results are available at: {path}");
            File.WriteAllBytes(path, csv);
            Console.WriteLine(@"Finished.");
            Console.ReadKey(false);
        }        
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ali.Analyze
{
    public static class LinqToCSV
    {
        public static string ToCsv<T>(this IEnumerable<T> items)
        where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();
            var index = 0;
            foreach (T item in items)
            {
                string line = string.Empty;
                if (index == 0)
                {
                    line += string.Join(",", properties.Select(p => p.Name.CreateHeader()).ToArray());
                    csvBuilder.AppendLine(line);
                }
                line = string.Join(",", properties.Select(p => p.GetValue(item, null).ToCsvValue()).ToArray());                
                csvBuilder.AppendLine(line);
                index++;
            }
            return csvBuilder.ToString();
        }

        private static string CreateHeader<T>(this T item)
        {
            if (item == null) return "\"\"";
            return string.Format("\"{0}\"", item);
        }

        private static string ToCsvValue<T>(this T item)
        {
            if (item == null) return "\"\"";

            if (item is string)
            {
                return string.Format("\"{0}\"", item.ToString().Replace("\"", "\\\""));
            }
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
            {
                return string.Format("{0}", item);
            }
            return string.Format("\"{0}\"", item);
        }
    }
}

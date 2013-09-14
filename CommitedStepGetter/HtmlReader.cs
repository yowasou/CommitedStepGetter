using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitedStepGetter
{
    public class HtmlReader
    {
        public string Header = string.Empty;
        public string Footter = string.Empty;
        public HtmlReader()
        {
            Header = "<html><head></head><body>\r\n";
            Footter = "</body></html>\r\n";
        }
        protected virtual string Read(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }
        public string CreateHTML(string revision,string csvFile, string templateFile,string templateheader)
        {
            Header = Read(templateheader);
            string template = Read(templateFile);
            StringBuilder sb = new StringBuilder(Header);
            StreamReader sr = new StreamReader(csvFile, System.Text.Encoding.GetEncoding("shift_jis"));
            try
            {
                string head = sr.ReadLine();
                string[] heads = head.Split(new char[] { ',' });
                int cFileName = Array.IndexOf(heads, "File Name");
                int cStatements = Array.IndexOf(heads, "Statements");
                int cMaxcomp = Array.IndexOf(heads, "Maximum Complexity*");
                while (!sr.EndOfStream)
                {
                    string[] readed = sr.ReadLine().Split(new char[] { ',' });
                    string outv = template.Replace("%filename%", readed[cFileName])
                        .Replace("%steps%", readed[cStatements])
                        .Replace("%maxcomp%", readed[cMaxcomp]);
                    sb.AppendLine(outv);
                }
                sb.AppendLine(Footter);
                sb.Replace("%revision%", revision);
            }
            finally
            {
                sr.Close();
            }
            return sb.ToString();
        }
        public void WriteHTML(string htmlfile, string contents)
        {
            StreamWriter sw = new StreamWriter(htmlfile,false,
                System.Text.Encoding.GetEncoding("shift_jis"));
            try
            {
                sw.Write(contents);
                sw.Flush();
            }
            finally
            {
                sw.Close();
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;

namespace CommitedStepGetter
{
    /// <summary>
    /// HTMLテンプレートファイルの読み込みと、CSVから読み込んでの変形、
    /// HTMLへの出力を行う
    /// </summary>
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
            TextFieldParser parser
                = new TextFieldParser(csvFile, Encoding.GetEncoding("Shift_JIS"));
            try
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                string[] heads = parser.ReadFields();
                int cFileName = Array.IndexOf(heads, "File Name");
                int cStatements = Array.IndexOf(heads, "Statements");
                int cMaxcomp = Array.IndexOf(heads, "Maximum Complexity*");
                while (parser.EndOfData == false)
                {
                    string[] readed = parser.ReadFields();
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
                parser.Close();
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

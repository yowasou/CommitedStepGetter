using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitedStepGetter
{
    public class XmlWriter
    {
        public string xmltemplate = string.Empty;
        public XmlWriter()
        {
            xmltemplate = @"<?xml version=""1.0"" encoding=""UTF-8"" ?>
<sourcemonitor_commands>
   <write_log>true</write_log>
   <command>
      <project_file>%projectpath%</project_file>
      <project_language>DCs</project_language>
      <modified_complexity>true</modified_complexity>
      <ignore_blank_lines>true</ignore_blank_lines>
      <source_directory>%temppath%</source_directory>
      <parse_utf8_files>True</parse_utf8_files>
      <checkpoint_name>Baseline</checkpoint_name>
      <checkpoint_date>1999-08-31T12:24:56</checkpoint_date>
      <show_measured_max_block_depth>True</show_measured_max_block_depth>
      <file_extensions>*.cs,*.resx</file_extensions>
      <include_subdirectories>true</include_subdirectories>
      <export>
        <export_file>%csvpath%</export_file>
        <export_type>3</export_type>
      </export>
   </command>
</sourcemonitor_commands>
";
        }
        public void Write(string cmdpath, string projectpath, string temppath, string csvpath)
        {
            StreamWriter sw = new StreamWriter(cmdpath);
            try
            {
                sw.Write(xmltemplate.Replace("%projectpath%",projectpath)
                    .Replace("%temppath%", temppath)
                    .Replace("%csvpath%",csvpath));
                sw.Flush();
            }
            finally
            {
                sw.Close();
            }
        }


    }
}

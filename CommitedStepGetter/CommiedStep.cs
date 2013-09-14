using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitedStepGetter
{
    public class CommitedStep
    {
        public virtual string Execute(string[] args)
        {
            string repoPath = args[1];
            string revNo = args[2];

            string svnpath = CommitedStepGetter.Properties.Settings.Default.svnpath;
            string svnlook = svnpath + "\\" + CommitedStepGetter.Properties.Settings.Default.svnlook;
            string temppath = CommitedStepGetter.Properties.Settings.Default.temppath;
            string smpath = CommitedStepGetter.Properties.Settings.Default.smpath;
            string smcmdpath = CommitedStepGetter.Properties.Settings.Default.smcmdpath;

            //変更されたファイルの一覧を取得
            string changedFiles = GetCommandResult(svnlook, "changed " + repoPath + " -r " + revNo);

            //Debug.Write(changedFiles); // ［出力］ウィンドウに出力

            //行ごとに配列へ
            string[] changeFilesLines = changedFiles.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> filesNames = new List<string>();
            foreach (string s in changeFilesLines)
            {
                string[] attrAndFile = s.Split(new string[] {" "},StringSplitOptions.RemoveEmptyEntries);
                if (attrAndFile.Length > 1)
                {
                    filesNames.Add(attrAndFile[1]);
                }
            }

            OutRevFilesToTempPath(repoPath, revNo, svnlook, temppath, filesNames);

            GetCommandResult(smpath, "/C \"" + smcmdpath + "\"");

            return "1";
        }

        /// <summary>
        /// TEMPPATHに指定したリビジョン、パスのファイルを出力
        /// </summary>
        /// <param name="repoPath"></param>
        /// <param name="revNo"></param>
        /// <param name="svnlook"></param>
        /// <param name="temppath"></param>
        /// <param name="filesNames"></param>
        protected virtual void OutRevFilesToTempPath(string repoPath, string revNo, string svnlook, string temppath, List<string> filesNames)
        {
            //ファイルをtempフォルダに吐く
            //例：svnlook cat C:\Repositories\svntest もくもく会.txt -r 13
            //一回フォルダを削除
            Directory.Delete(temppath, true);
            //また作成
            Directory.CreateDirectory(temppath);

            foreach (string fileName in filesNames)
            {
                string fileCat = GetCommandResult(svnlook, "cat " + repoPath + " " + fileName + " -r " + revNo);
                string directoryName = Path.GetDirectoryName(temppath + "\\" + fileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                StreamWriter sw = new StreamWriter(temppath + "\\" + fileName);
                try
                {
                    sw.Write(fileCat);
                    sw.Flush();
                }
                finally
                {
                    sw.Close();
                }
            }
        }
        
        public virtual string GetCommandResult(string command, string arg)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = command; // 実行するファイル
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.Arguments = arg;
            psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
            Process p = Process.Start(psInfo); // アプリの実行開始
            string output = p.StandardOutput.ReadToEnd(); // 標準出力の読み取り
            output = output.Replace("\r\r\n", "\n"); // 改行コードの修正
            return output;
        }
    }
}

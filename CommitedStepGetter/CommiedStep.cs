using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //変更されたファイルの一覧を取得
            string changedFiles = GetCommandResult(svnlook, "changed " + repoPath + " -r " + revNo);

            Debug.Write(output); // ［出力］ウィンドウに出力
            return "1";
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

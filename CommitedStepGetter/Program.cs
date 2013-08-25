using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitedStepGetter
{
    class Program
    {
        /// <summary>
        /// 0 : バッチファイルのあるフォルダ
        /// 1 : コミットしたローカルのリポジトリパス
        /// 2 : コミットしたリビジョン番号
        /// 3 : ???（前回リビジョン - a?）
        /// cd "C:\Program Files (x86)\SourceMonitor"
        /// SourceMonitor.exe /DCs C:\temp\testcode\temp.cs
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CommitedStep cs = new CommitedStep();
            Console.WriteLine(cs.Execute(args));
            return;
        }
    }
}

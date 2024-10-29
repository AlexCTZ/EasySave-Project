using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projet_V1._0
{
    class ProgressBar
    {
        private const int ProgressBarLength = 50;
        private int total;

        public void SetTotal(int totalFiles)
        {
            total = totalFiles;
        }

        public void Report(double percentage)
        {
            int progress = (int)(percentage * ProgressBarLength);
            string progressBar = new string('#', progress) + new string('-', ProgressBarLength - progress);

            Console.Write($"\r[{progressBar}] {percentage * 100:F2}%");
        }
    }
}

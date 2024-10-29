using System;
using System.Collections.Generic;
using System.Text;

namespace Projet_V1._0
{
    public class BackupJob
    {
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public BackupType Type { get; set; }
    }

    public enum BackupType
    {
        Full,
        Differential
    }
}

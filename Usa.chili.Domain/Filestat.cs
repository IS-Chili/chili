using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Filestat
    {
        public string FileName { get; set; }
        public ulong FileSize { get; set; }
        public DateTime StatTime { get; set; }
    }
}

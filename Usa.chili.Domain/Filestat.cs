using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class Filestat
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime StatTime { get; set; }
    }
}

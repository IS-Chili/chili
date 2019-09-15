using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Filestat
    {
        [Key]
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime StatTime { get; set; }
    }
}

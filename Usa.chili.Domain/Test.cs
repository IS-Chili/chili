using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class Test
    {
        [Key]
        public long RecId { get; set; }
        public DateTime? Ts { get; set; }
    }
}

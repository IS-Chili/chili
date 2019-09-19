using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class MobileusawQcRain
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public float? TbPrecip { get; set; }
        public int? Tb1dayQcCode { get; set; }
        public int? Tb5minQc11 { get; set; }
        public int? Tb5minQc20 { get; set; }
        public int? Tb5minQc22 { get; set; }
        public float? TxPrecip { get; set; }
        public int? Tx1dayQcCode { get; set; }
        public int? Tx5minQc11 { get; set; }
        public int? Tx5minQc20 { get; set; }
        public int? Tx5minQc22 { get; set; }
    }
}

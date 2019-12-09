using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class VariableType
    {
        public VariableType()
        {
            VariableDescription = new HashSet<VariableDescription>();
        }

        public uint Id { get; set; }
        public string VariableType1 { get; set; }
        public decimal MetricMin { get; set; }
        public decimal MetricMax { get; set; }
        public string MetricUnit { get; set; }
        public string MetricSymbol { get; set; }
        public decimal EnglishMin { get; set; }
        public decimal EnglishMax { get; set; }
        public string EnglishUnit { get; set; }
        public string EnglishSymbol { get; set; }

        public virtual ICollection<VariableDescription> VariableDescription { get; set; }
    }
}

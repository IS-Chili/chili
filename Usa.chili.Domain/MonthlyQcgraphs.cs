using System;
using System.ComponentModel.DataAnnotations;

namespace Usa.chili.Domain
{
    public partial class MonthlyQcgraphs
    {
        public string Year { get; set; }
        public string Month { get; set; }
    }
}

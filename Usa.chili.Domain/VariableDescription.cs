using System;
using System.Collections.Generic;

namespace Usa.chili.Domain
{
    public partial class VariableDescription
    {
        public uint Id { get; set; }
        public uint VariableTypeId { get; set; }
        public string VariableName { get; set; }
        public string VariableDescription1 { get; set; }

        public virtual VariableType VariableType { get; set; }
    }
}

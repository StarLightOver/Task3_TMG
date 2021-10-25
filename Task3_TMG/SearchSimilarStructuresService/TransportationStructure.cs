using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task3_TMG
{
    public class TransportationStructure
    {
        public string Line { set; get; }
        
        public double IndexLine { set; get; }
        
        public IEnumerable<TransportationStructure>? AttachedLines { set; get; }

        public override bool Equals(object? obj)
        {
            return obj is TransportationStructure structure
                   && Line.Equals(structure.Line)
                   && IndexLine.Equals(structure.IndexLine)
                   && (AttachedLines == null && structure.AttachedLines == null
                       || AttachedLines != null && structure.AttachedLines != null && AttachedLines.SequenceEqual(structure.AttachedLines));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Task3_TMG;

namespace Tests
{
    public class TransportationStructureDataBuilder
    {
        private readonly PetrenkoGoltzmanMethod _model = new();
        
        private string RussianLine { set; get; }
        
        private double IndexLine { set; get; }
        
        private List<TransportationStructure> BuildingObject { set; get; }

        public TransportationStructureDataBuilder(string russianLine)
        {
            RussianLine = russianLine;
            IndexLine = _model.GetIndex(russianLine);
            BuildingObject = new List<TransportationStructure>();
        }

        public TransportationStructureDataBuilder AddAttachedLine(IEnumerable<string> attachedLine)
        {
            BuildingObject.Add(new TransportationStructure()
            {
                Line = RussianLine,
                IndexLine = IndexLine,
                AttachedLines = attachedLine.Select(line => new TransportationStructure()
                {
                    Line = line,
                    IndexLine = _model.GetIndex(line),
                    AttachedLines = null,
                }).ToList<TransportationStructure>()
            });

            return this;
        }
        
        public IEnumerable<TransportationStructure> Build()
        {
            return BuildingObject;
        }
    }
}
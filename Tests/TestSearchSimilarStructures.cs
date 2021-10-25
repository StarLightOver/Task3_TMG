using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Task3_TMG;
using FluentAssertions;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    public class TestSearchSimilarStructures
    {
        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void PetrenkoGoltzmanMethod_1Return_Success()
        {
            var russianLines = new[]
            {
                "Привет"
            };
            
            var englishLines = new[]
            {
                "qwe",
                "qwer",
                "qwert",
            };

            var expectedLines = new TransportationStructureDataBuilder(russianLines.First())
                .AddAttachedLine(new List<string> {"qwert", "qwer", "qwe"})
                .Build()
                .ToArray();
            
            CheckLines(new TestData()
            {
                RussianLines = russianLines,
                EnglishLines = englishLines,
                ExpectedLines = expectedLines
            });
        }
        
        [Test]
        public void PetrenkoGoltzmanMethod_MoreThen1Return_Success()
        {
            var russianLines = new[]
            {
                "Привет"
            };
            
            var englishLines = new[]
            {
                "",
                "qwe",
                "qwer",
                "qwert",
                "rewq",
                "aser",
                "<>",
                "qw",
                "as",
                "qw",
            };

            var expectedLines = new TransportationStructureDataBuilder(russianLines.First())
                .AddAttachedLine(new List<string> {"aser", "rewq", "qwer", "qw", "as", "qw"})
                .AddAttachedLine(new List<string> {"qwert", "qwer", "qwe"})
                .AddAttachedLine(new List<string> {"qwert", "rewq", "qwe"})
                .AddAttachedLine(new List<string> {"qwert", "aser", "qwe"})
                .Build()
                .ToArray();
            
            CheckLines(new TestData()
            {
                RussianLines = russianLines,
                EnglishLines = englishLines,
                ExpectedLines = expectedLines
            });
        }
        
        [Test]
        public void PetrenkoGoltzmanMethod_ReturnEmpty_Success()
        {
            var russianLines = new[]
            {
                "Привет"
            };
            
            var englishLines = new[]
            {
                "",
                "qwertyqwerty",
            };

            var expectedLines = System.Array.Empty<TransportationStructure>();
            
            CheckLines(new TestData()
            {
                RussianLines = russianLines,
                EnglishLines = englishLines,
                ExpectedLines = expectedLines
            });
        }
        
        [Test]
        public void PetrenkoGoltzmanMethod_WithComments_Success()
        {
            var russianLines = new[]
            {
                "Привет"
            };
            
            var englishLines = new[]
            {
                " | qwerty",
                "qwertyqwerty | qwerty",
            };

            var expectedLines = System.Array.Empty<TransportationStructure>();
            
            CheckLines(new TestData()
            {
                RussianLines = russianLines,
                EnglishLines = englishLines,
                ExpectedLines = expectedLines
            });
        }

        private void CheckLines(TestData data)
        {
            // Arrange
            var model = new PetrenkoGoltzmanMethod();

            var service = new SearchSimilarStructuresService(model);
            
            var engMasWithoutComments = data.EnglishLines.Select(line =>
            {
                var lastIndex = line.LastIndexOf('|');
                return lastIndex >= 0 ? line.Remove(lastIndex) : line;
            }).ToArray();
            
            // Act
            var result = service.GetSimilarLines(data.RussianLines, engMasWithoutComments);
            
            // Assert
            Assert.AreEqual(result, data.ExpectedLines);
        }
    }

    public class TestData
    {
        public string[] RussianLines { set; get; }
        
        public string[] EnglishLines { set; get; }
        
        public IEnumerable<TransportationStructure> ExpectedLines { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Task3_TMG
{
    class Program
    {
        private static readonly string[] RusMas = new[]
        {
            "Привет Миры! 12:) ?",
        };

        private static string[] EngMas = new[]
        {
            "|Comment", // 0
            "ab|Comment Comment", // 2
            "abc|Comment Comment Comment", // 3
            "abc|Comment Comment Comment Comment", // 3
            "abcd|Comment Comment Comment Comment Comment", // 4
            "abcde|Comment", // 5
            "[]|Comment", // 0
            "edcba|Comment", // 5
            "abcdef|Comment", // 6
            "abcdefg|Comment", // 7
            ";:|/?|Comment", // 0
            "qwertyaser", // 10
        };

        static void Main(string[] args)
        {
            var model = new PetrenkoGoltzmanMethod();

            var service = new SearchSimilarStructuresService(model);
            
            // Подчищаем комментарии к входным строкам
            var engMasWithoutComments = EngMas.Select(line =>
            {
                var lastIndex = line.LastIndexOf('|');
                return lastIndex >= 0 ? line.Remove(lastIndex) : line;
            }).ToArray();
            
            var result = service.GetSimilarLines(RusMas, engMasWithoutComments);
            foreach (var structure in result)
            {
                Console.WriteLine( $"\"{structure.Line}\" (Index = {structure.IndexLine}):");
                
                if (structure.AttachedLines != null)
                    foreach (var item in structure.AttachedLines)
                    {
                        Console.WriteLine( $"\t\"{item.Line}\" (Index = {item.IndexLine});");
                    }

                Console.WriteLine();
            }
        }
    }
}
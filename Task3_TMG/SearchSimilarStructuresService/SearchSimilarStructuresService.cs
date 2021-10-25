using System;
using System.Collections.Generic;
using System.Linq;

namespace Task3_TMG
{
    public class SearchSimilarStructuresService
    {
        
        private readonly IGettingLineIndex IndexCalculationModel;
        
        public SearchSimilarStructuresService(IGettingLineIndex model)
        {
            IndexCalculationModel = model;
        }
        
        /// <summary>
        /// Метод находит подходящий набор английских строк для каждой русской строки из входного набора.
        /// В качестве арбитража для строк выступает установленная модель.
        /// </summary>
        /// <param name="russianLines">Исходный набор русских слов</param>
        /// <param name="englishLines">Исходный набор английских слов</param>
        /// <returns>Набор структур TransportationStructure </returns>
        public IEnumerable<TransportationStructure> GetSimilarLines(IEnumerable<string> russianLines, IEnumerable<string> englishLines)
        {
            // Будем работать с массивами
            // Вес - это индекс каждой строки, вычесленный по определенному методу (вес = индекс)
            var arrayEnglishLines = englishLines.ToArray();
            var weightEnglishLines = arrayEnglishLines.Select(x => IndexCalculationModel.GetIndex(x)).ToArray();
            
            // Сортируем для того, чтобы на каждом шаге мы знали, что вес следующего шага будет не меньше! текущего
            Array.Sort(weightEnglishLines, arrayEnglishLines);
            
            // Список, хрянящий наборы строк, которые суммарно имеют индекс равный индексу русской строки
            var result = new List<TransportationStructure>();
            
            // Для каждой русской строки
            foreach (var russianLine in russianLines)
            {
                // Получаем вес русской строки
                var weightRussianLine = IndexCalculationModel.GetIndex(russianLine);
                
                // Стек будет накапливать суммарный вес английских строк
                var stack = new Stack<int>();
                
                // Получаем индекс первой строки, которая имеет индекс больше 0
                // Этот индекс будет считаться стартовым
                // Например: для ["", "a", "b"] стартовый индекс будет 1, следовательно элемент с индексом 0 никогда не будет обработан
                var indexOfMasWeight = weightEnglishLines.Select((item, index) => (item, index)).First(x => x.item != 0).index;
                
                // В цикле идем до тех пор, пока не обработаем каждую строку
                // (Пока индекс массива не выйдет за его длину уменьшенную на единицу)
                while (indexOfMasWeight < weightEnglishLines.Length)
                {
                    // Определяем текущую сумму последовательности
                    var currentSumIndex = stack.Aggregate(0d, (acc, y) => acc += weightEnglishLines[y]);
                    
                    // Определяем доступный остаток по индексу
                    var residue = weightRussianLine - (currentSumIndex + weightEnglishLines[indexOfMasWeight]);
                    
                    // Если остаток 0, то мы нашли подходящую строку, добавляем ее в результирующий набор
                    if (residue == 0)
                    {
                        stack.Push(indexOfMasWeight);
                        
                        var atachedLines = stack.ToList().Select(x => new TransportationStructure()
                        {
                            Line = arrayEnglishLines[x],
                            IndexLine = weightEnglishLines[x],
                            AttachedLines = null,
                        });
                        
                        result.Add(new TransportationStructure()
                        {
                            Line = russianLine,
                            IndexLine = weightRussianLine,
                            AttachedLines = atachedLines,
                        });
                        
                        stack.Pop();
                    }
                    
                    // Если остаток больше текущего веса, то запоминаем текущий элемент (добавляем его в стек)
                    if (residue >= weightEnglishLines[indexOfMasWeight])
                    {
                        stack.Push(indexOfMasWeight);
                    }
                    
                    // Извлекать из стека будем в двух случаях:
                    // 1) Если стек не пуст и остаток отрицательный 
                    // 2) Если стек не пуст и дошли до конца массива
                    if (stack.Count != 0 && (residue < 0d || indexOfMasWeight == weightEnglishLines.Length - 1))
                    {
                        indexOfMasWeight = stack.Pop();
                    }
                    
                    // Это условие помогает выйти раньше из цикла в ситуации, когда все строки имеют индекс больше искомого
                    // Например: Искамый индекс 10. Массив индексов для иностранных слов [20, 30, 100, 100500].
                    // Так как массив всегда отсортирован, можно утверждать что если стек пуст и остаток отрицательный, то 
                    // любой следующий элемент будет приводить к такому же текущему состоянию.
                    if (stack.Count == 0 && residue < 0d) break;

                    indexOfMasWeight++;
                }
            }
            
            return result;
        }
    }
}
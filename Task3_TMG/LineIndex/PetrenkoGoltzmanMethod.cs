using System;
using System.Linq;

namespace Task3_TMG
{
    public class PetrenkoGoltzmanMethod : IGettingLineIndex
    {
        /// <summary>
        /// Метод находит индекс по методу Петренко – Гольцмана
        /// </summary>
        /// <param name="line">Исходная строка</param>
        /// <returns>Значение индекса по методу Петренко – Гольцмана</returns>
        /// Описание метода:
        /// Индекс Петренко для первой буквы в строке равен 0.5, для каждой последующей – на единицу больше.
        /// Индекс Петренко для строки равен сумме индексов составляющих её букв, дополнительно умноженной на длину строки.
        /// Все разделительные символы, включая знаки препинания, дефисы, апострофы и пробелы, при расчёте индекса не учитываются. 
        public double GetIndex(string line)
        {
            var countLetterInLine = line.Count(letter => char.IsLetter(letter));
            
            var sumFrom0ToLineSizeMinus1 = Sum(countLetterInLine - 1);

            var index = (sumFrom0ToLineSizeMinus1 + countLetterInLine * 0.5d) * countLetterInLine;
            
            return index;
        }

        /// <summary>
        /// Метод находит индекс оптимальным способом
        /// </summary>
        /// <param name="line">Исходная строка</param>
        /// <returns>Значение индекса по методу Петренко – Гольцмана</returns>
        public int GetOptimalIndex(string line) => line.Count(letter => char.IsLetter(letter));

        /// <summary>
        /// Находит сумму чисел от 1 до end
        /// </summary>
        private static double Sum(double end)
        {
            return (end + 1d) / 2d * end;
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var bigrams = text
                .Where(t => t.Count > 1)
                .SelectMany(sentence => sentence
                    .Zip(sentence.Skip(1), (key, value) => (key, value)));

            var trigrams = text
                .Where(t => t.Count > 2)
                .SelectMany(sentence => sentence
                    .Zip(sentence.Skip(1), (first, second) => first + " " + second)
                    .Zip(sentence.Skip(2), (key, value) => (key, value)));

            return bigrams
                .Concat(trigrams)
                .ToLookup(x => x.key, x => x.value)
                .ToDictionary(x => x.Key, x => x.GetMaxValue());
        }

        private static string GetMaxValue(this IGrouping<string, string> values)
        {
            var sortedValues = values
                .ToLookup(x => values.Count(y => y == x))
                .OrderByDescending(x => x.Key)
                .First();

            return sortedValues.Count() > 1 
                ? sortedValues.Aggregate((first, second) => string.CompareOrdinal(first, second) < 0 ? first : second)
                : sortedValues.First();
        }
    }
}


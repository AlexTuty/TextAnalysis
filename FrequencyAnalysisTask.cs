using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class FrequencyAnalysisTask
    {
        private enum TypeNgram
        {
            Default,
            Bigram,
            Trigram
        }

        public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
        {
            var result = new Dictionary<string, string>();
            var intermedia = new Dictionary<string, Dictionary<string, int>>();
            text = text.Where(t => t.Count > 1).ToList();
            BuildNgram(text, intermedia, TypeNgram.Bigram);
            text = text.Where(t => t.Count > 2).ToList();
            BuildNgram(text, intermedia, TypeNgram.Trigram);
            SortNgram(intermedia, result);
            return result;
        }

        private static void BuildNgram(
            List<List<string>> text,
            Dictionary<string, Dictionary<string, int>> intermedia,
            TypeNgram nGram)
        {
            foreach (var sentence in text)
            {
                for (int i = 0; i < sentence.Count - (int)nGram; i++)
                {
                    var key = default(string);
                    var value = default(string);
                    switch (nGram)
                    {
                        case TypeNgram.Bigram:
                            key = sentence[i];
                            value = sentence[i + 1];
                            break;
                        case TypeNgram.Trigram:
                            key = sentence[i] + " " + sentence[i + 1];
                            value = sentence[i + 2];
                            break;
                    }
                    AddNgram(intermedia, key, value);
                }
            }
        }

        private static void AddNgram(
            Dictionary<string, Dictionary<string, int>> intermedia,
            string key, string value)
        {
            if (!intermedia.ContainsKey(key))
                intermedia[key] = new Dictionary<string, int>() { { value, 1 } };
            else if (!intermedia[key].ContainsKey(value))
                intermedia[key][value] = 1;
            else
                intermedia[key][value]++;
        }

        private static void SortNgram(
            Dictionary<string, Dictionary<string, int>> intermedia,
            Dictionary<string, string> result)
        {
            foreach (var ngram in intermedia.Keys)
            {
                if (intermedia.TryGetValue(ngram, out Dictionary<string, int> keyValues))
                    result[ngram] = SearchMax(keyValues);
                else
                    continue;
            }
        }

        private static string SearchMax(Dictionary<string, int> dicti)
        {
            var result = dicti.OrderByDescending(k => k.Value).First();
            foreach (var item in dicti)
            {
                if (item.Value == result.Value && string.CompareOrdinal(item.Key, result.Key) < 0)
                    result = item;
            }

            return result.Key;
        }
    }
}


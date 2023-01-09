using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(
            Dictionary<string, string> nextWords,
            string phraseBeginning,
            int wordsCount)
        {
            if (nextWords.Count == 0)
                return phraseBeginning;
            if (wordsCount == 0 && nextWords.GetEnumerator().MoveNext())
                return nextWords.Keys.First();

            var nextWord = new List<string>() { phraseBeginning };
            for (int i = 1; i <= wordsCount; i++)
            {
                var ngram = ReturnNgram(string.Join(" ", nextWord));
                if (!TryGetValueNextWord(nextWords, ngram, out string value))
                    break;
                nextWord.Add(value);
            }
            return string.Join(" ", nextWord);
        }

        internal static bool TryGetValueNextWord(Dictionary<string, string> nextWords, string[] ngram, out string exitWord)
        {
            return nextWords.TryGetValue(ngram[0], out exitWord)
                || ngram.Length >= 2 && nextWords.TryGetValue(ngram[1], out exitWord);
        }

        internal static string[] ReturnNgram(string phraseBeginning)
        {
            var nextWord = phraseBeginning.Split(' ');
            return nextWord.Length >= 2
                ? new string[]
                {
                    nextWord[nextWord.Length - 2] + " " + nextWord[nextWord.Length - 1],
                    nextWord[nextWord.Length - 1]
                }
                : new[] { nextWord[nextWord.Length - 1] };
        }
    }
}
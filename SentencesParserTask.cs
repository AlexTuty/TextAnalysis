using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class SentencesParserTask
    {
        public static List<List<string>> ParseSentences(string text)
        {
            var separators = new[] { '.', '!', '?', ';', ':', '(', ')' };

            return text
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(sentence => sentence.Parse())
                .Where(sentence => sentence.Count != 0)
                .ToList();
        }

        private static List<string> Parse(this string sentence)
        {
            return sentence
                .ReplaceNotLetter()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private static string ReplaceNotLetter(this string sentence)
        {
            return new string(sentence
                .Select(ch => char.IsLetter(ch) || ch == '\'' ? char.ToLower(ch) : ' ')
                .ToArray());
        }
    }
}


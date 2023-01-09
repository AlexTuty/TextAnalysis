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
                .ParseSentences()
                .Select(sentence => sentence
                    .ToLower()
                    .ReplaceNotLetter(' ')
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList())
                .ToList();
        }

        private static IEnumerable<string> ParseSentences(this IEnumerable<string> sentences)
        {
            var str = default(string);
            foreach (var sentence in sentences)
            {
                if (sentence.All(c => !char.IsLetter(c)))
                    continue;
                else if (sentence.All(c => char.IsUpper(c)) && sentence.Length > 1)
                    str += sentence;
                else if (str != null)
                {
                    yield return str;
                    str = default;
                }
                else
                    yield return sentence;
            }
        }

        private static string ReplaceNotLetter(this string str, char newChar)
        {
            foreach (var item in str)
            {
                if (!char.IsLetter(item) && item != '\'')
                    str = str.Replace(item, newChar);
            }
            return str;
        }
    }
}
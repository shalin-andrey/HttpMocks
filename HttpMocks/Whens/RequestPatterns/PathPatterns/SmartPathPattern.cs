using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HttpMocks.Whens.RequestPatterns.PathPatterns
{
    public sealed class SmartPathPattern : IHttpRequestPathPattern
    {
        private readonly Regex regex;
        private readonly Dictionary<string, string> rxPatterns = new Dictionary<string, string>
        {
            {"@guid", "[A-Fa-f0-9\\-]+"},
            {"@str", ".+"}
        };

        public SmartPathPattern(string pattern)
        {
            regex = BuildRegex(Normalize(pattern));
        }

        bool IHttpRequestPathPattern.IsMatch(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            return regex.IsMatch(Normalize(value));
        }

        private Regex BuildRegex(string pattern)
        {
            var regexPattern = rxPatterns.Aggregate(pattern, (current, rx) => current.Replace(rx.Key, rx.Value));
            return new Regex($"^{regexPattern}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        private static string Normalize(string path)
        {
            return path.Trim('/', ' ').ToLower();
        }
    }
}
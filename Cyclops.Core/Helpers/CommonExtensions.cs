using System;
using System.Collections.Generic;
using System.Linq;
using Cyclops.Xmpp.Protocol;

namespace Cyclops
{
    public static class CommonExtensions
    {
        public static IEnumerable<ResultPair> SplitAndIncludeDelimiters(this string text, string[] delimiters)
        {
            if (delimiters.Length == 0)
            {
                yield return new ResultPair { String = text };
                yield break;
            }
            delimiters = delimiters.OrderByDescending(i => i.Length).ToArray();

            string[] split = text.Split(delimiters, StringSplitOptions.None);

            foreach (string part in split)
            {
                if (!string.IsNullOrEmpty(part))
                    yield return new ResultPair { String = part };
                text = text.Substring(part.Length);

                string delim = delimiters.FirstOrDefault(x => text.StartsWith(x));
                if (delim != null)
                {
                    if (!string.IsNullOrEmpty(delim))
                        yield return new ResultPair { String = delim, IsDelimiter = true };
                    text = text.Substring(delim.Length);
                }
            }
        }

        public class ResultPair
        {
            public string String { get; set; }
            public bool IsDelimiter { get; set; }
        }

        public static bool BaresEqual(this Jid x, Jid y)
        {
            return string.Equals(x.Local, y.Local, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(x.Domain, y.Domain, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool StringToBool(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return str.ToLower().Equals("true");
        }
    }
}

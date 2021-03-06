﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;

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

        public static string StatusTypeToString(this StatusType type)
        {
            switch(type)
            {
                case StatusType.Online:
                    return "online";
                case StatusType.Busy:
                    return "dnd";
                case StatusType.Away:
                    return "away";
                case StatusType.ExtendedAway:
                    return "xa";
                case StatusType.Chat:
                    return "chat";
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public static bool BaresEqual(this IEntityIdentifier x, IEntityIdentifier y)
        {
            return x.User.Equals(y.User, StringComparison.InvariantCultureIgnoreCase) &&
                   x.Server.Equals(y.Server, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool StringToBool(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            return str.ToLower().Equals("true");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using DateAccess.Services.ContactService.Call;

namespace DateAccess.Services
{
    public static class Util
    {
        /// <summary>
        /// synchronize access to method in the case of concurrency / racing condition
        /// give up after number of retries
        /// retry interval in milliseconds
        /// </summary>
        /// <typeparam name="T">type of synchronization class for a different locking field</typeparam>
        internal class Synchronize<T> where T : ICallService
        {
            private static int _locking;

            public static TResult Run<TResult>(Func<TResult> func, int numRetries, int retryInterval)
            {
                var tried = 0;

                do
                {
                    tried++;

                    if (Interlocked.Exchange(ref _locking, 1) == 0)
                    {
                        var result = func();
                        Interlocked.Exchange(ref _locking, 0);
                        return result;
                    }
                    //sleep off the interval
                    Thread.Sleep(retryInterval);
                } while (tried < numRetries);

                return default(TResult);
            }
        }

        /// <summary>
        /// calculate the similarity between two string in probability
        /// intend to use for fuzzy matching in database migration
        /// </summary>
        public static class StringSimilarity
        {
            public static double Compare(string str1, string str2)
            {
                var pair1 = WordPairs(str1.ToUpper());
                var pair2 = WordPairs(str2.ToUpper());

                var intersect = 0;
                var totalCount = pair1.Count + pair2.Count;

                foreach (var pairInPair1 in pair1)
                {
                    for (var i = 0; i < pair2.Count; i++)
                    {
                        if (pairInPair1 == pair2[i])
                        {
                            intersect++;
                            pair2.RemoveAt(i);
                            break;
                        }
                    }
                }

                return (2.0*intersect)/totalCount;
            }

            private static IList<string> WordPairs(string str)
            {
                var pairs = new List<string>();
                var words = Regex.Split(str, @"\s");

                foreach (var value in words)
                {
                    if (string.IsNullOrEmpty(value))
                        continue;

                    var pairsInWord = LetterPair(value);
                    pairs = pairs.Concat(pairsInWord).ToList();
                }

                return pairs;
            }

            private static IEnumerable<string> LetterPair(string str)
            {
                var size = str.Length - 1;
                var pairs = new string[size];
                for (var i = 0; i < size; i++)
                {
                    pairs[i] = str.Substring(i, 2);
                }

                return pairs;
            }
        }
    }
}

﻿using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using WireMock.Types;
using Stef.Validation;

namespace WireMock.Matchers.Request
{
    /// <summary>
    /// The request header matcher.
    /// </summary>
    /// <inheritdoc cref="IRequestMatcher"/>
    public class RequestMessageHeaderMatcher : IRequestMatcher
    {
        private readonly MatchBehaviour _matchBehaviour;
        private readonly bool _ignoreCase;

        /// <summary>
        /// The functions
        /// </summary>
        public Func<IDictionary<string, string[]>, bool>[] Funcs { get; }

        /// <summary>
        /// The name
        /// </summary>
        public string Name { get; }

        /// <value>
        /// The matchers.
        /// </value>
        public IStringMatcher[] Matchers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        /// <param name="matchBehaviour">The match behaviour.</param>
        public RequestMessageHeaderMatcher(MatchBehaviour matchBehaviour, [NotNull] string name, [NotNull] string pattern, bool ignoreCase)
        {
            Guard.NotNull(name, nameof(name));
            Guard.NotNull(pattern, nameof(pattern));

            _matchBehaviour = matchBehaviour;
            _ignoreCase = ignoreCase;
            Name = name;
            Matchers = new IStringMatcher[] { new WildcardMatcher(matchBehaviour, pattern, ignoreCase) };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="matchBehaviour">The match behaviour.</param>
        /// <param name="name">The name.</param>
        /// <param name="patterns">The patterns.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        public RequestMessageHeaderMatcher(MatchBehaviour matchBehaviour, [NotNull] string name, bool ignoreCase, [NotNull] params string[] patterns) :
            this(matchBehaviour, name, ignoreCase, patterns.Select(pattern => new WildcardMatcher(matchBehaviour, pattern, ignoreCase)).Cast<IStringMatcher>().ToArray())
        {
            Guard.NotNull(patterns, nameof(patterns));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="matchBehaviour">The match behaviour.</param>
        /// <param name="name">The name.</param>
        /// <param name="matchers">The matchers.</param>
        /// <param name="ignoreCase">Ignore the case from the pattern.</param>
        public RequestMessageHeaderMatcher(MatchBehaviour matchBehaviour, [NotNull] string name, bool ignoreCase, [NotNull] params IStringMatcher[] matchers)
        {
            Guard.NotNull(name, nameof(name));
            Guard.NotNull(matchers, nameof(matchers));

            _matchBehaviour = matchBehaviour;
            Name = name;
            Matchers = matchers;
            _ignoreCase = ignoreCase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessageHeaderMatcher"/> class.
        /// </summary>
        /// <param name="funcs">The funcs.</param>
        public RequestMessageHeaderMatcher([NotNull] params Func<IDictionary<string, string[]>, bool>[] funcs)
        {
            Guard.NotNull(funcs, nameof(funcs));

            Funcs = funcs;
        }

        /// <inheritdoc cref="IRequestMatcher.GetMatchingScore"/>
        public double GetMatchingScore(IRequestMessage requestMessage, RequestMatchResult requestMatchResult)
        {
            double score = IsMatch(requestMessage);
            return requestMatchResult.AddScore(GetType(), score);
        }

        private double IsMatch(IRequestMessage requestMessage)
        {
            if (requestMessage.Headers == null)
            {
                return MatchBehaviourHelper.Convert(_matchBehaviour, MatchScores.Mismatch);
            }

            // Check if we want to use IgnoreCase to compare the Header-Name and Header-Value(s)
            var headers = !_ignoreCase ? requestMessage.Headers : new Dictionary<string, WireMockList<string>>(requestMessage.Headers, StringComparer.OrdinalIgnoreCase);

            if (Funcs != null)
            {
                return MatchScores.ToScore(Funcs.Any(f => f(headers.ToDictionary(entry => entry.Key, entry => entry.Value.ToArray()))));
            }

            if (Matchers == null)
            {
                return MatchScores.Mismatch;
            }

            if (!headers.ContainsKey(Name))
            {
                return MatchBehaviourHelper.Convert(_matchBehaviour, MatchScores.Mismatch);
            }

            WireMockList<string> list = headers[Name];
            return Matchers.Max(m => list.Max(m.IsMatch)); // TODO : is this correct ?
        }
    }
}
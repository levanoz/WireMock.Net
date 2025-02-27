﻿using System.Collections.Generic;

namespace WireMock.Admin.Mappings
{
    /// <summary>
    /// Cookie Model
    /// </summary>
    [FluentBuilder.AutoGenerateBuilder]
    public class CookieModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the matchers.
        /// </summary>
        public IList<MatcherModel> Matchers { get; set; }

        /// <summary>
        /// Gets or sets the ignore case.
        /// </summary>
        public bool? IgnoreCase { get; set; }

        /// <summary>
        /// Reject on match.
        /// </summary>
        public bool? RejectOnMatch { get; set; }
    }
}
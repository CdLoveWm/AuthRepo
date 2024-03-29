﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerAuth.Policy
{
    public class CustomClaimsAuthorizationRequirement : AuthorizationHandler<CustomClaimsAuthorizationRequirement>, IAuthorizationRequirement
    {
        /// <summary>
        /// Creates a new instance of <see cref="ClaimsAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="claimType">The claim type that must be present.</param>
        /// <param name="allowedValues">The optional list of claim values, which, if present, 
        /// the claim must match.</param>
        public CustomClaimsAuthorizationRequirement(string claimType, IEnumerable<string> allowedValues)
        {
            if (claimType == null)
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            ClaimType = claimType;
            AllowedValues = allowedValues;
        }

        /// <summary>
        /// Gets the claim type that must be present.
        /// </summary>
        public string ClaimType { get; }

        /// <summary>
        /// Gets the optional list of claim values, which, if present, 
        /// the claim must match.
        /// </summary>
        public IEnumerable<string> AllowedValues { get; }

        /// <summary>
        /// Makes a decision if authorization is allowed based on the claims requirements specified.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomClaimsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var found = false;
                if (requirement.AllowedValues == null || !requirement.AllowedValues.Any())
                {
                    found = context.User.Claims.Any(c => string.Equals(c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    found = context.User.Claims.Any(c => string.Equals(c.Type, requirement.ClaimType, StringComparison.OrdinalIgnoreCase)
                                                        && requirement.AllowedValues.Contains(c.Value, StringComparer.Ordinal));
                }
                if (found)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}

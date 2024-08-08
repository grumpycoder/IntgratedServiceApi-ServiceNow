using Microsoft.AspNetCore.Authorization;

namespace IntegratedServicesApi.Web;


    public static class AuthorizationPolicyBuilderExtentions
    {
        public static void AddPolicyScope_AllowedScopes(this AuthorizationOptions authorizationOptions, string policyName, params string[] allowedScopes) =>
            authorizationOptions.AddPolicy(policyName, policyBuilder => policyBuilder.RequireClaim("scope", allowedScopes));


        public static void AddPolicyScope_AllRequiredScopes(this AuthorizationOptions authorizationOptions, string policyName, params string[] requiredScopes) =>
            authorizationOptions.AddPolicy(policyName, policyBuilder => policyBuilder.RequireScopes(requiredScopes));

        public static AuthorizationPolicyBuilder RequireScopes(this AuthorizationPolicyBuilder builder, params string[] requiredScopes) =>
            builder.RequireAssertion(context => {
                var userScopes = GetUserScopes(context);
                return requiredScopes.All(scope => userScopes.Contains(scope, StringComparer.CurrentCulture));
            });

        private static IEnumerable<string> GetUserScopes(this AuthorizationHandlerContext context) =>
            context?.User?
                .Claims
                .Where(c => c.Type.Equals("scope"))
                .SelectMany(c => c.Value.Split(' ')) ?? new List<string>();
    }

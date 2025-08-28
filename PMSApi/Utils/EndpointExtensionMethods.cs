using Microsoft.AspNetCore.Authorization;
using PMSApi.Enums;

namespace PMSApi.Utils
{
    public static class EndpointExtensionMethods
    {
      
        public static RouteHandlerBuilder SetRequiredAccessLevel(
            this RouteHandlerBuilder endpoint,
            AccessLevel accessLevel
        ) => endpoint.WithMetadata(accessLevel);

 
        public static AccessLevel GetAccessLevel(this Endpoint endpoint)
        {
            var accessLevel = endpoint.Metadata.FirstOrDefault(x => x.GetType() == typeof(AccessLevel));
            return accessLevel != null ? (AccessLevel)accessLevel : AccessLevel.None;
        }

        public static bool HasAccessLevel(this Endpoint endpoint, AccessLevel userAccessLevel) =>
            endpoint.GetAccessLevel() <= userAccessLevel;

        public static bool IsAnnonymous(this Endpoint endpoint) =>
            endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
    }

}

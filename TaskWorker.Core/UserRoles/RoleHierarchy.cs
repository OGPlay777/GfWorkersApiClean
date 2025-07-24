namespace OperationWorker.Core.UserRoles
{
    public static class RoleHierarchy
    {
        private static readonly Dictionary<string, List<string>> RoleAccessMap = new()
        {
            ["Guest"] = new() { "Guest" },
            ["Worker"] = new() { "Worker", "Guest" },
            ["Supervisor"] = new() { "Supervisor", "Worker", "Guest" },
            ["Office"] = new() { "Office", "Supervisor", "Worker", "Guest" },
            ["Admin"] = new() { "Admin", "Supervisor", "Worker", "Guest", "Office" }
        };

        public static bool HasAccess(string userRole, string requiredRole)
        {
            if (userRole == null || requiredRole == null) return false;
            return RoleAccessMap.TryGetValue(userRole, out var roles) && roles.Contains(requiredRole);
        }
    }
}
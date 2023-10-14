using System.Reflection;

namespace UserSecurity.API.ServiceDefinition
{
    public static class ServiceDefinitionInfo
    {
        public static Assembly Assembly => typeof(ServiceDefinitionInfo).Assembly;

        public static readonly string Name = "ServiceDefinitionInfo";
    }
}

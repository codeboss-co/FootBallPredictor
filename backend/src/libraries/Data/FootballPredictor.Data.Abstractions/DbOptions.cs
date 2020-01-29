using System.Collections.Generic;
using System.Reflection;

namespace FootballPredictor.Data.Abstractions
{
    public class DbOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public AssemblyList Assemblies { get; set; }
    }

    public class AssemblyList : List<Assembly>
    {
        public AssemblyList(params Assembly[] assemblies)
        {
            AddRange(assemblies);
        }
    }
}

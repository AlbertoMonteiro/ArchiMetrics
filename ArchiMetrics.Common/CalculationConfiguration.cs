using System.Collections.Generic;
using System.Linq;

namespace ArchiMetrics.Common
{
    public class CalculationConfiguration
    {
        private IEnumerable<string> namespacesIgnored = Enumerable.Empty<string>();
        private IEnumerable<string> typesIgnored = Enumerable.Empty<string>();

        public IEnumerable<string> NamespacesIgnored
        {
            get { return namespacesIgnored; }
            set { namespacesIgnored = value; }
        }

        public IEnumerable<string> TypesIgnored
        {
            get { return typesIgnored; }
            set { typesIgnored = value; }
        }
    }
}
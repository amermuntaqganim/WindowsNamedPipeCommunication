using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeServerWService
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() => new T(), LazyThreadSafetyMode.ExecutionAndPublication);

        private Singleton()
        {
           
        }

        public static T Instance => instance.Value;
    }
}

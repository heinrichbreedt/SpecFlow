using System;
using System.Collections.Generic;
using System.Linq;

namespace TechTalk.SpecFlow
{
    public abstract class SpecFlowContext : Dictionary<string, object>, IDisposable
    {
        protected virtual void Dispose()
        {
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        public bool TryGetValue<TValue>(string key, out TValue value)
        {
            object result;
            if (base.TryGetValue(key, out result))
            {
                value = (TValue)result;
                return true;
            }

            value = default(TValue);
            return false;
        }

        public void Set<T>(T data)
        {
            var id = typeof(T).ToString();
            Set(data, id);
        }

        public void Set<T>(T data, string id)
        {
            this[id] = data;
        }

        public T Get<T>() where T : class
        {
            var id = typeof(T).ToString();
            return Get<T>(id);
        }

        public T Get<T>(string id) where T : class
        {
            return this[id] as T;
        }
    }
}
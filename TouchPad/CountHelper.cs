using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KyleOlson.TouchPad
{

    class CountHelper<T, V>  where V : KeyedItem<T>
    {
        Dictionary<T, CountedItem> items = new Dictionary<T, CountedItem>();

        public CountHelper()
        {

        }

        public bool Add(T key, V value)
        {
            if (items.TryGetValue(key, out CountedItem ci))
            {
                ci.Count++;
                return false;
            }
            else
            {
                ci = new CountedItem(value);
                items[key] = ci;
                return true;
            }
        }

        public bool Remove(T key)
        {
            if (items.TryGetValue(key, out CountedItem ci))
            {
                ci.Count--;
                if (ci.Count <= 0)
                {
                    items.Remove(key);
                    return true;
                }
            }
            return false;
            
        }

        private class CountedItem
        {
            public int Count { get; set; } = 1;
            public V Value { get; set; }
            public CountedItem(V value)
            {
                Value = value;
            }

            

        }
    }

    public interface KeyedItem<T>
    {
         T Key {get;}
    }
}

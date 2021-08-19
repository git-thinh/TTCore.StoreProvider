using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.Models
{
    public class CollectionItems<T>
    {
        public T[] Items { set; get; }
    }
}

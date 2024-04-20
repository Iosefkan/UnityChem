using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.Scripts.UI
{
    public class MyList<T> : List<T>
    {
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return this.SequenceEqual(obj as List<T>);
        }
    }
}

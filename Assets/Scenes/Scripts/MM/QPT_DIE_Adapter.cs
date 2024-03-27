using Program;
using System.Xml.Serialization;

namespace Extruder
{
    internal class QPT_DIE_Adapter
    {
        public void init(InitData id)
        {
            qpt = new QPT();
            die= new DIE();

            qpt.init(id);

            id.res = qpt.Res;
            die.init(id);

            id.dop.Q = die.RES_f.Q_fin*1e-6;
            qpt.init(id, true);
        }

        public void init()
        {
            init(initData);
        }

        public QPT qpt = new QPT();
        public DIE die = new DIE();
        public InitData initData;
    }
}

using Program;

namespace Extruder
{
    internal class QPT_DIE_Adapter
    {
        public void init(InitData initData)
        {
            qpt = new QPT();
            die= new DIE();

            qpt.init(initData);

            initData.res = qpt.Res;
            die.init(initData);

            initData.dop.Q = die.RES_f.Q_fin*1e-6;
            qpt.init(initData, true);
        }

        public QPT qpt = new QPT();
        public DIE die = new DIE();
    }
}

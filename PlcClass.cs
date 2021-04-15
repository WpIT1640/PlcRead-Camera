using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead4
{
    public class PlcClass
    {

       public static Tag<BoolPlcMapper,bool> dryerStack1 = new Tag<BoolPlcMapper, bool>()
            {
                Name = "E321531EVO01.ZSO",
                Gateway = "172.30.201.3",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };
        public static Tag<BoolPlcMapper,bool> dryerStack2 = new Tag<BoolPlcMapper, bool>()
        {
            Name = "E322531EVO01.ZSO",
            Gateway = "172.30.201.3",
            Path = "1,0",
            PlcType = PlcType.ControlLogix,
            Protocol = Protocol.ab_eip,
            Timeout = TimeSpan.FromSeconds(5)
        };
        public static Tag<BoolPlcMapper, bool> ventStack1 = new Tag<BoolPlcMapper, bool>()
        {
            Name = "D301070EV01.ZSO",
            Gateway = "172.30.201.2",
            Path = "1,1",
            PlcType = PlcType.ControlLogix,
            Protocol = Protocol.ab_eip,
            Timeout = TimeSpan.FromSeconds(5)
        };
        public static Tag<BoolPlcMapper, bool> ventStack2 = new Tag<BoolPlcMapper, bool>()
        {
            Name = "D302070EV01.ZSO",
            Gateway = "172.30.201.2",
            Path = "1,1",
            PlcType = PlcType.ControlLogix,
            Protocol = Protocol.ab_eip,
            Timeout = TimeSpan.FromSeconds(5)
        };


    }
}

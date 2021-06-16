using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCRead4
{
    /*
Tags would be:
Emergency stacks PLC is 1756-L72 ControlLogix5572, IP 172.30.201.3
dryer 1 stack is opened E321531EVO01.ZSO
dryer 1 stack is closed E321531EVO01.ZSC
dryer 2 stack is opened E322531EVO01.ZSO
dryer 2 stack is closed E322531EVO01.ZSC

Vent stacks PLC is 1756-L71 ControlLogix5571, IP 172.30.201.2
dryer 1 stack is opened D301070EV01.ZSO
dryer 1 stack is closed D301070EV01.ZSC
dryer 2 stack is opened D302070EV01.ZSO
dryer 2 stack is closed D302070EV01.ZSC
*/
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
        public static List<Tag<BoolPlcMapper, bool>> listOfTags = new List<Tag<BoolPlcMapper, bool>>();		
    }
}

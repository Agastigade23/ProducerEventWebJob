using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerEventWebJob
{
    public interface IProducer
    {
       string ProduceChargeEvent();
    }
}

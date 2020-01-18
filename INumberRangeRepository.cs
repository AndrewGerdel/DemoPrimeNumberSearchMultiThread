using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPrimeNumberSearchMultiThread
{
    interface INumberRangeRepository
    {
        void GenerateNumberRange(out long lowNumberRange, out long highNumberRange);
    }
}

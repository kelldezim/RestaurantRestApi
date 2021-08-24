using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRestApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message): base(message)
        {

        }
    }
}

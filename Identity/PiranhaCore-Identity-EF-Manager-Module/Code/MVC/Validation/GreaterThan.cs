using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class GreaterThanAttribute : IsAttribute
    {
        public GreaterThanAttribute(string dependentProperty) : base(Operator.GreaterThan, dependentProperty) { }
    }
}

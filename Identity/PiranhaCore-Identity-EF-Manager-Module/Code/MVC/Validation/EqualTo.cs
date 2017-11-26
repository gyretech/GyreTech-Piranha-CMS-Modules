using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class EqualToAttribute : IsAttribute
    {
        public EqualToAttribute(string dependentProperty) : base(Operator.EqualTo, dependentProperty) { }
    }
}

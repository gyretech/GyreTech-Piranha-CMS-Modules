using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        public RequiredIfNotAttribute(string dependentProperty, object dependentValue) : base(dependentProperty, Operator.NotEqualTo, dependentValue) { }
    }
}

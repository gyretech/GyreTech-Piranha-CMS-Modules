﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piranha.AspNetCore.Identity.EF.Manager
{
    public class NotEqualToAttribute : IsAttribute
    {
        public NotEqualToAttribute(string dependentProperty) : base(Operator.NotEqualTo, dependentProperty) { }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret_Application.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Role { get; }
    }
}

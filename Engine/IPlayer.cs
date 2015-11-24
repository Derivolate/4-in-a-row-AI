﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IPlayer
    {
        players player { get; }
        byte get_turn(Field field);
    }
}

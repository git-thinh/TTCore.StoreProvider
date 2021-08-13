﻿using System;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.Interfaces
{
    public interface IClock
    {
        Task ShowTime(DateTime currentTime);
    }
}

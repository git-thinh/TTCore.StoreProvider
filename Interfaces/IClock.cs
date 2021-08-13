﻿using System;
using System.Threading.Tasks;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Interfaces
{
    public interface IClock
    {
        Task ShowTime(DateTime currentTime);
        Task Send(MessageDto data);
    }
}

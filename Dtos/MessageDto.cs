﻿using System;

namespace TTCore.StoreProvider.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }
    }
}

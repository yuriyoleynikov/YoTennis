﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoTennis.Models;

namespace YoTennis.Services
{
    public interface IMatchService
    {
        Task AddEventAsync(GameEvent gameEvent);
        Task<MatchModel> GetStateAsync();
        Task Reset();
        Task UndoAsync();
    }
}

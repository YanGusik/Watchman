﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Devscord.DiscordFramework.Framework.Architecture.Middlewares
{
    public interface IMiddleware<T> where T : IDiscordContext
    {
        T Process(SocketMessage data);
    }
}

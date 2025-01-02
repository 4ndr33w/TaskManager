﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Services
{
    public class DesksRequestService : BaseRequestService<DeskDto>
    {
        protected override string GetApiUrlString()
        {
            return HOST + desksApiUrl;
        }
    }
}

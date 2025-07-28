using HRSDataIntegration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Interfaces
{
    public interface IRabbitMqService
    {
        void SendMessage(MessageDTO messages);
    }
}

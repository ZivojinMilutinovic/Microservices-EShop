
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Dtos;
using UserMicroservice.Models;

namespace UserMicroservice.AsyncDataService
{
    public interface IMessageBusClient
    {
        void PublishNewUser(UserPublishedDto user);
    }
}

using ProductMicroservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        public void PublishNewProduct(GetProductDto getProductDto)
        {
            throw new NotImplementedException();
        }
    }
}

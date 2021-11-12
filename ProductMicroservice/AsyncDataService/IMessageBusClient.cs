using ProductMicroservice.Dtos;
using ProductMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.AsyncDataService
{
    public interface IMessageBusClient
    {
        void PublishNewProduct(InventoryPostProductDto product);
    }
}

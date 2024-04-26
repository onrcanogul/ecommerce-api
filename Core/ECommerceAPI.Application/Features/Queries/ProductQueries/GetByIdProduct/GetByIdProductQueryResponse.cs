﻿using ECommerceAPI.Application.DTOs.Product;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.ProductQueries.GetByIdProduct
{
    public class GetByIdProductQueryResponse
    {
        public ProductDto Product { get; set; }
    }
}

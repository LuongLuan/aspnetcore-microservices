﻿using AutoMapper;
using MediatR;
using Ordering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrUpdateCommand : IRequest<ApiResult<long>>
    {
        public decimal TotalPrice { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string ShippingAddress { get; set; }

        private string _invoiceAddress;
        public string? InvoiceAddress
        {
            get => _invoiceAddress;
            set => _invoiceAddress = value ?? ShippingAddress;
        }
    }
}

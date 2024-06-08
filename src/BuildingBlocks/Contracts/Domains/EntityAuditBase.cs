using Contracts.Domains.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Domains
{
    public class EntityAuditBase<T> : EntityBase<T>, IAuditable
    {
        public DateTimeOffset CreatedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? LastModifiedDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

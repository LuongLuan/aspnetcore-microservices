using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Common.Interfaces
{
    public interface IUnitOfWork<TContex> : IDisposable where TContex : DbContext
    {
        Task<int> CommitAsync();
    }
}

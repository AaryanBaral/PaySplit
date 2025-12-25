using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PaySplit.Application.Common.Abstractions;
using PaySplit.Application.Common.Results;

namespace PaySplit.Application.Tenants.Command.UpdateTenant
{
    public class RenameTenantCommand : ICommand<Result<RenameTenantResult>>
    {
        public string Name { get; }
        public Guid Id { get; }
        public RenameTenantCommand(Guid id, string name)
        {
            Name = name;
            Id = id;
        }
    }
}

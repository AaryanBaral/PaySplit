using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Command.UpdateTenant
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
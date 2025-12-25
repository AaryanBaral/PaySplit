using Application.Common.Abstractions;
using Application.Common.Results;

namespace Application.Tenants.Command.CreateTenant
{
    public class CreateTenantCommand:ICommand<Result<CreateTenantResult>>
    {
        public string Name {get;} = default!;

        public CreateTenantCommand(string name)
        {
            Name = name;
        }
    }
}

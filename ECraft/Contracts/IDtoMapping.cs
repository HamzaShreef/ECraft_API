using Microsoft.AspNetCore.Identity;

namespace ECraft.Contracts
{
	public interface IDtoMapping<TDto,TDomainEntity>
	{
		TDomainEntity GetDomainEntity(out bool successfulMapping,out IdentityError? validationError,TDomainEntity? oldInstance);

		TDto GetDto(TDomainEntity domainEntity);
	}
}

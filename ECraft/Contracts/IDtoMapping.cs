namespace ECraft.Contracts
{
	public interface IDtoMapping<TDto,TDomainEntity>
	{
		TDomainEntity GetDomainEntity(out bool successfulMapping,out string validationErrorMessage,TDomainEntity? oldInstance);

		TDto GetDto(TDomainEntity domainEntity);
	}
}

using Microsoft.AspNetCore.Identity;

namespace ECraft.Domain
{
	public class ErrorList : List<IdentityError>
	{
		public void AddError(string code, string description)
		{
			if (code == null || description == null)
			{
				throw new ArgumentNullException();
			}

			IdentityError error = new IdentityError()
			{
				Code = code,
				Description = description
			};

			Add(error);
		}

	}
}

using ECraft.Constants;

namespace ECraft.Domain
{
	public class PaginationFilter
	{
        public int PageNumber { get; set; }

		public int PageSize { get; set; }

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 4;
		}

		public PaginationFilter(int pageNumber,int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = Math.Min(pageSize, SizeConstants.GenericPageSize);
        }
    }
}

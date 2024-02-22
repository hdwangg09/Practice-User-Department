namespace Server.Common.Response
{
    public class Paging
    {
        public Paging(int PageSize, int TotalRow, int PageIndex)
        {
            TotalRecord = TotalRow;
            this.PageSize = PageSize;
            this.PageIndex = PageIndex;
            if (TotalRow > 0)
            {
                var LastPageCal = TotalRow / PageSize > 0 ? TotalRow / PageSize : 0;
                if (TotalRow % PageSize != 0) LastPageCal += 1;
                LastPage = LastPageCal;
            }
        }

        public Paging(int TotalRow, int PageIndex)
        {
            TotalRecord = TotalRow;
            this.PageIndex = PageIndex;
            if (TotalRow > 0)
            {
                var LastPageCal = TotalRow / PageSize > 0 ? TotalRow / PageSize : 0;
                if (TotalRow % PageSize != 0) LastPageCal += 1;
                LastPage = LastPageCal;
            }
        }

        public int PageIndex { get; set; }
        public int LastPage { get; set; }
        public int TotalRecord { get; set; }
        public int PageSize { get; set; } = 10;
    }
}

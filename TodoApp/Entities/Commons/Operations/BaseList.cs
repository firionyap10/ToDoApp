using TodoApp.API.Entities.Enums;

namespace TodoApp.API.Entities.Commons.Operations
{
    public class BaseList
    {
        private const int _maxPageSize = 50;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
        }

        public BaseListOrderBy OrderBy { get; set; } = BaseListOrderBy.Descending;

        public string OrderByName { get; set; } = "CreatedDate";
    }
}

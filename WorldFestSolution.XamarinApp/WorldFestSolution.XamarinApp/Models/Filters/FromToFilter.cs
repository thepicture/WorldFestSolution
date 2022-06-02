using System.Collections.Generic;
using System.Linq;

namespace WorldFestSolution.XamarinApp.Models.Filters
{
    public class FromToFilter : IFromToFilter
    {
        public int From { get; set; }
        public int To { get; set; }
        public string Title { get; set; }
        public SortType SortType { get; set; }
        public string PropertyName { get; set; }

        public IEnumerable<TTarget> Accept<TTarget>(IEnumerable<TTarget> items)
        {
            IEnumerable<TTarget> paginatedItems = items
                .ToList()
                .Skip(From - 1)
                .Take(To - From + 1);
            if (SortType == SortType.Ascending)
            {
                return paginatedItems.OrderBy(i =>
                {
                    return i.GetType()
                            .GetProperty(PropertyName)
                            .GetValue(i);
                });
            }
            else
            {
                return paginatedItems.OrderByDescending(i =>
                {
                    return i.GetType()
                            .GetProperty(PropertyName)
                            .GetValue(i);
                });
            }
        }
    }
}

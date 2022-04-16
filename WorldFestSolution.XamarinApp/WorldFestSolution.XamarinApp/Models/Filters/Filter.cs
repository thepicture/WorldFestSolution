using System.Collections.Generic;
using System.Linq;

namespace WorldFestSolution.XamarinApp.Models.Filters
{
    public class Filter : IFilter
    {
        public string Title { get; set; }
        public SortType SortType { get; set; }
        public string PropertyName { get; set; }
        public IEnumerable<TTarget> Accept<TTarget>(IEnumerable<TTarget> items)
        {
            if (SortType == SortType.Ascending)
            {
                return items.OrderBy(i =>
                {
                    return i.GetType()
                            .GetProperty(PropertyName)
                            .GetValue(i);
                });
            }
            else
            {
                return items.OrderByDescending(i =>
                {
                    return i.GetType()
                            .GetProperty(PropertyName)
                            .GetValue(i);
                });
            }
        }
    }
}

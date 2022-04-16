using System.Collections.Generic;

namespace WorldFestSolution.XamarinApp.Models.Filters
{
    public interface IFilter
    {
        string Title { get; set; }
        SortType SortType { get; set; }
        string PropertyName { get; set; }
        IEnumerable<TTarget> Accept<TTarget>(IEnumerable<TTarget> items);
    }
}

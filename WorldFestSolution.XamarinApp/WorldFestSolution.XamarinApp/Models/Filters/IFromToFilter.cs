namespace WorldFestSolution.XamarinApp.Models.Filters
{
    /// <summary>
    /// Defines filtering including from and to criterions.
    /// </summary>
    public interface IFromToFilter : IFilter
    {
        int From { get; set; }
        int To { get; set; }
    }
}

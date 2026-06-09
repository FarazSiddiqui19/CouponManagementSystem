namespace CouponManagementSystem.Data.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int TotalItems { get; }

    public PaginatedList(List<T> items, int totalItems)
    {
        Items = items;
        TotalItems = totalItems;
    }
}

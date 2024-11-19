using HackatonTaskLib.Utils;
using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.WishlistGeneration
{
    public class EmployeeRandomWishlistGenerator : IEmployeeWishlistGenerator
    {
        public Wishlist Generate(int employeeId, int[] employeeIds)
        {
            var ids = employeeIds.ToList();
            ids.Shuffle();
            return new Wishlist(employeeId, ids.ToArray());
        }
    }
}

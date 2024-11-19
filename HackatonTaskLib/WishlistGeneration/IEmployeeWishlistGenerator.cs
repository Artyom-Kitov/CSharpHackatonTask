using Nsu.HackathonProblem.Contracts;

namespace HackatonTaskLib.WishlistGeneration
{
    public interface IEmployeeWishlistGenerator
    {
        Wishlist Generate(int employeeId, int[] employeeIds);
    }
}

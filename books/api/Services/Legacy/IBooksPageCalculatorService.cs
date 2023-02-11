
// Theoretically this interface could/should be in a separate (legacy) project, but it's not worth the effort

namespace BooksApi.Services.Legacy;

public interface IBooksPageCalculatorService
{
    int CalculatePageCount(Guid bookId);
}

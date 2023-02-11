
// Theoretically this class could/should be in a separate (legacy) project, but it's not worth the effort

using System.Diagnostics;

namespace BooksApi.Services.Legacy;

public class BooksPageCalculatorService : IBooksPageCalculatorService
{
    private const int MIN_PAGE_COUNT = 1;
    private const int MAX_PAGE_COUNT = 1000;
    
    private const int DELAY_IN_MILLISECONDS = 5000;

    private readonly Random _random = new Random();

    public int CalculatePageCount(Guid bookId)
    {
        // we're simulating a legacy service that takes a long time to calculate the page count,
        // what is important is that this service is CPU-bound, not I/O-bound,
        // so we're using a simple while loop to simulate the CPU-bound work
        
        var watch = new Stopwatch();
        watch.Start();
        while (true)
        {
            if (watch.ElapsedMilliseconds > DELAY_IN_MILLISECONDS)
            {
                break;
            }
        }

        return _random.Next(MIN_PAGE_COUNT, MAX_PAGE_COUNT + 1);
    }
}
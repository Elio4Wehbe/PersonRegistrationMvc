using PersonRegistrationMvc.Models;

namespace PersonRegistrationMvc.ViewModels
{
    public class TopBookStat
    {
        public string Title { get; set; }
        public int BorrowCount { get; set; }
    }

    public class ReportsViewModel
    {
        public int TotalBooks { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrowings { get; set; }
        public int OverdueBooks { get; set; }
        public int BorrowsThisMonth { get; set; }
        public int BorrowsLastMonth { get; set; }

        public int TrendMonths { get; set; }

        public List<TopBookStat> MostBorrowedBooks { get; set; } = new();

        public List<string> TrendMonthLabels { get; set; } = new();
        public List<int> TrendMonthCounts { get; set; } = new();

        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryCounts { get; set; } = new();

        public List<string> ActiveUserLabels { get; set; } = new();
        public List<int> ActiveUserCounts { get; set; } = new();

        public static ReportsViewModel Build(List<Book> books, List<Person> persons, List<Borrow> borrows, int trendMonths)
        {
            var now = DateTime.UtcNow;
            var lastMonth = now.AddMonths(-1);

            var viewModel = new ReportsViewModel
            {
                TotalBooks = books.Count,
                TotalMembers = persons.Count,
                ActiveBorrowings = borrows.Count(b => b.ReturnDate == null),
                OverdueBooks = borrows.Count(b => b.ReturnDate == null && b.DueDate < now),
                BorrowsThisMonth = borrows.Count(b => b.BorrowDate.Year == now.Year && b.BorrowDate.Month == now.Month),
                BorrowsLastMonth = borrows.Count(b => b.BorrowDate.Year == lastMonth.Year && b.BorrowDate.Month == lastMonth.Month),
                TrendMonths = trendMonths
            };

            viewModel.MostBorrowedBooks = borrows
                .Where(b => b.Book != null)
                .GroupBy(b => b.Book.Title)
                .Select(g => new TopBookStat { Title = g.Key, BorrowCount = g.Count() })
                .OrderByDescending(x => x.BorrowCount)
                .Take(5)
                .ToList();

            for (int i = trendMonths - 1; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                viewModel.TrendMonthLabels.Add(month.ToString("MMM yyyy"));
                viewModel.TrendMonthCounts.Add(
                    borrows.Count(b => b.BorrowDate.Year == month.Year && b.BorrowDate.Month == month.Month));
            }

            var categoryGroups = borrows
                .Where(b => b.Book?.Category != null)
                .GroupBy(b => b.Book.Category.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();
            viewModel.CategoryLabels = categoryGroups.Select(x => x.Name).ToList();
            viewModel.CategoryCounts = categoryGroups.Select(x => x.Count).ToList();

            var userGroups = borrows
                .Where(b => b.Person != null)
                .GroupBy(b => b.Person.FullName)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();
            viewModel.ActiveUserLabels = userGroups.Select(x => x.Name).ToList();
            viewModel.ActiveUserCounts = userGroups.Select(x => x.Count).ToList();

            return viewModel;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScriptureJournal.Models;

namespace ScriptureJournal
{
    public class IndexModel : PageModel
    {
        private readonly ScriptureJournal.Data.ScriptureJournalContext _context;

        public IndexModel(ScriptureJournal.Data.ScriptureJournalContext context)
        {
            _context = context;
        }

        public string BookSort { get; set; }
        public string DateAddedSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public IList<Entry> Entry { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Book {get; set;}
        [BindProperty(SupportsGet = true)]
        public string EntryBook { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            BookSort = String.IsNullOrEmpty(sortOrder) ? "Book" : "";
            DateAddedSort = sortOrder == "DAdded_Asc_Sort" ? "DAdded_Desc_Sort" : "DAdded_Asc_Sort";

            IQueryable<string> bookListQuery = from e in _context.Entry
                                               orderby e.Book
                                               select e.Book;

           CurrentFilter = searchString;

            IQueryable<Entry> bookQuery = from e in _context.Entry select e;

            if (!String.IsNullOrEmpty(searchString))
            {//Genre = Book Title = note? (Copy from razorpages assignment
                bookQuery = bookQuery.Where(s => s.Note.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(EntryBook))
            {
                bookQuery = bookQuery.Where(x => x.Book == EntryBook);
            }
            
            switch (sortOrder)
            {
                case "Book":
                    bookQuery = bookQuery.OrderByDescending(s => s.Book);
                    break;
                case "DAdded_Asc_Sort":
                    bookQuery = bookQuery.OrderBy(s => s.DateAdded);
                    break;
                case "DAdded_Desc_Sort":
                    bookQuery = bookQuery.OrderByDescending(s => s.DateAdded);
                    break;
                default:
                    bookQuery = bookQuery.OrderBy(s => s.Book);
                    break;+
            }
            Book = new SelectList(await bookListQuery.Distinct().ToListAsync());
            Entry = await bookQuery.AsNoTracking().ToListAsync();
        }
    }
}

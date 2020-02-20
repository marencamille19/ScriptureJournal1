using System;
using System.ComponentModel.DataAnnotations;

namespace ScriptureJournal.Models
{
    public class Entry{
        public int ID{ get; set; }
        public string Book { get; set; }
        
        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
        public string Note { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace CQRSStarter.DAL.Entities
{
    public class Todo : EntityBase
    {
        [Required]
        public string Text { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

}
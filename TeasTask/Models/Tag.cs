using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TeasTask.Models;

namespace TestTask.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string PictureName { get; set; }
        public byte[] Image { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public Article()
        {
            Tags = new List<Tag>();
            Categories = new List<Category>();
        }
    }
}

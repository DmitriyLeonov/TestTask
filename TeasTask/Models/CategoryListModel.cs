using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestTask.Models;

namespace TestTask.Models
{
    public class CategoryListModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestTask.Models
{
    public class ArticleListModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public SelectList Categories { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
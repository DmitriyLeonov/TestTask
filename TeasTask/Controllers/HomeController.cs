using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        ArticleContext context = new ArticleContext();

        public ActionResult Index(int? category, int page = 1, DateTime? startdate = null, DateTime? enddate = null)
        {
            IEnumerable<Article> articles = context.Articles.Include("Categories");
            int pageSize = 6;
            IList<Article> articlesList = new List<Article>();
            if (category != null & category != 0)
            {
                foreach(var article in articles)
                {
                    foreach(Category c in article.Categories)
                    {
                        if(c.CategoryId == category)
                        {
                            articlesList.Add(article);
                        }
                    }
                }
            }
            else
            {
                articlesList = articles.ToList();
            }
            if (startdate != null)
            {
                articlesList = articles.Where(a => a.Date.Date >= startdate).ToList();
            }
            if (enddate != null)
            {
                articlesList = articles.Where(a => a.Date.Date <= enddate).ToList();
            }
            IEnumerable<Article> articlesPerPage = articlesList.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber=page, PageSize=pageSize, TotalItems=articlesList.Count()};
            List<Category> CategoryList =  context.Categories.ToList();
            CategoryList.Insert(0, new Category{ CategoryName = "All", CategoryId = 0});
            ArticleListModel articleList = new ArticleListModel
            {
                Articles = articlesPerPage.ToList(),
                Categories = new SelectList(CategoryList,"CategoryId","CategoryName"),
                PageInfo = pageInfo
            };
            //var articles = context.Articles;
            return View(articleList);
        }

        public ActionResult ArticleDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = context.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }
    }
}
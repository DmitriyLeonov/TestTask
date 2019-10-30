using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeasTask.Models;
using TestTask.Models;

namespace TeasTask.Controllers
{
    public class ArticlesController : Controller
    {
        private ArticleContext db = new ArticleContext();

        // GET: Articles
        public ActionResult Index(int page = 1)
        {
            int pageSize = 20;
            IEnumerable<Article> articles = db.Articles.Include("Categories");
            IEnumerable<Article> articlesPerPage = articles.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber=page, PageSize=pageSize, TotalItems=articles.Count()};
            List<Category> CategoryList =  db.Categories.ToList();
            CategoryList.Insert(0, new Category{ CategoryName = "All", CategoryId = 0});
            ArticleListModel articleList = new ArticleListModel
            {
                Articles = articlesPerPage.ToList(),
                Categories = new SelectList(CategoryList,"CategoryId","CategoryName"),
                PageInfo = pageInfo
            };
            return View(articleList);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ShortDescription,Description,Date,CategoryId,PictureNmae,Image")] Article article, int selectedCategory, HttpPostedFileBase uploadImage)
        {
            article.Categories = new List<Category>();
            if(selectedCategory != null)
            {
                foreach (var c in db.Categories.Where(co => selectedCategory == co.CategoryId))
                    {
                        article.Categories.Add(c);
                    }
            }
            article.Date = DateTime.Now;
            if(uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                article.Image = imageData;
            }
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name"/*, article.CategoryId*/);
            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.Categories.ToList();
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ShortDescription,Description,Date")] Article article, int selectedCategory)
        {
            if(selectedCategory != null)
            {
                foreach (var c in db.Categories.Where(co => selectedCategory == co.CategoryId))
                    {
                        article.Categories.Clear();
                        article.Categories.Add(c);
                    }
            }
            article.Date = DateTime.Now;
            bool saveFailed;
                do
                {
                    saveFailed = false;

                    try
                    {
                        if (ModelState.IsValid)
                        {
                            Article newArticle = new Article();
                            newArticle = (from a in db.Articles where a.Name == article.Name select a).Single();
                            newArticle.Description = article.Description;
                            newArticle.Name = article.Name;
                            newArticle.ShortDescription = article.ShortDescription;
                            newArticle.Categories.Clear();
                            newArticle.Categories = article.Categories;
                            db.Entry(newArticle).State = EntityState.Modified;
                            db.SaveChanges();
                            
                        }
                        //ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name"/* article.CategoryId*/);
                        
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        Article newArticle = new Article();
                        newArticle = (from a in db.Articles where a.Name == article.Name select a).Single();
                        article.ArticleId = newArticle.ArticleId;
                    }

                } while (saveFailed);
                return RedirectToAction("Index");
                return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void HaveUserResolveConcurrency(DbPropertyValues currentValues,
                                       DbPropertyValues databaseValues,
                                       DbPropertyValues resolvedValues)
        {
            currentValues.GetValue<Article>("ArticleId");
            resolvedValues.SetValues(db.Articles.Find().Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

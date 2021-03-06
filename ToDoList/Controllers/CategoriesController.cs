using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult Create(string name, string description)
    {
      Category newCategory = new Category(name, description);
      newCategory.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Show(int id)
    {
      Category selectedCategory = Category.Find(id);
      selectedCategory.Items = Item.FindForCategory(id);
      return View(selectedCategory);
    }

    // This one creates new Items within a given Category, not new Categories:
    [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemTitle, string itemDescription, DateTime due)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemTitle, itemDescription, due, categoryId);
      newItem.Save();    // New code
      List<Item> categoryItems = Item.FindForCategory(categoryId);
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

    [HttpPost("/categories/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Category.Delete(id);
      return RedirectToAction("Index");
    }
  }
}
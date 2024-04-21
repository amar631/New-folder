using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuickKartDataAccessLayer;
using QuickKartDataAccessLayer.Models;


namespace QuickKartServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class ProductController : Controller
    {
        QuickKartRepository repository;
        public ProductController()
        {
            repository = new QuickKartRepository();
        }

        //public JsonResult GetAllProducts()
        //{
        //    List<Products> products = new List<Products>();
        //    return Json(products);
        //}

        [HttpGet]
        public JsonResult GetAllProducts()
        {
            List<Products> products = new List<Products>();
            try
            {
                products = repository.GetAllProducts();
            }
            catch (Exception ex)
            {
                products = null;
            }
            return Json(products);
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            List<Categories> categories = new List<Categories>();
            try
            {
                categories = repository.GetAllCategories();
            }
            catch (Exception ex)
            {

                categories = null;
            }
            return Json(categories);
        }
        [HttpPost]
        public JsonResult AddProductUsingParams(string productName, byte categoryId, decimal price, int quantityAvailable)
        {
            bool status = false;
            string productId;
            string message;
            try
            {
                status = repository.AddProduct(productName, categoryId, price, quantityAvailable, out productId);
                if (status)
                {
                    message = "Successful addition operation, ProductId = " + productId;
                }
                else
                {
                    message = "Unsuccessful addition operation!";
                }
            }
            catch (Exception ex)
            {
                message = "Some error occured, please try again!";
            }
            return Json(message);
        }

        [HttpPost]
        public JsonResult AddProductByModels(Products product)
        {
            bool status = false;
            string message;

            try
            {
                status = repository.AddProduct(product);
                if (status)
                {
                    message = "Successful addition operation, ProductId = " + product.ProductId;
                }
                else
                {
                    message = "Unsuccessful addition operation!";
                }
            }
            catch (Exception)
            {
                message = "Some error occured, please try again!";
            }
            return Json(message);
        }

        public JsonResult InsertCategory(string categoryName)
        {
            string message;
            bool status = false;
            try
            {
                status = repository.AddCategory(categoryName);
                if (status)
                {
                    message = "Added Successfully";
                }
                else
                {
                    message = "some error occured";
                }
            }
            catch (Exception ex)
            {

                message = "error";
            }
            return Json(message);
        }
        [HttpPut]
        public bool UpdateProductByEFModels(Products product)
        {
            bool status = false;

            try
            {
                status = repository.UpdateProduct(product);
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        [HttpPut]
        public bool UpdateProductByAPIModels(Models.Products product)
        {
            bool status = false;

            try
            {
                if (ModelState.IsValid)
                {
                    Products prodObj = new Products();
                    prodObj.ProductId = product.ProductId;
                    prodObj.ProductName = product.ProductName;
                    prodObj.CategoryId = product.CategoryId;
                    prodObj.Price = product.Price;
                    prodObj.QuantityAvailable = product.QuantityAvailable;
                    status = repository.UpdateProduct(prodObj);
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        [HttpPut]
        public bool UpdateCategory(Models.Categories category)
        {
            bool status = false;
            try
            {
                if (ModelState.IsValid)
                {
                    Categories catObj = new Categories();
                    catObj.CategoryId = category.CategoryId;
                    catObj.CategoryName = category.CategoryName;
                    status = repository.UpdateCategory(catObj);
                    status = repository.UpdateCategory(catObj);
                }
            }
            catch (Exception)
            {

                status = false;
            }
            return status;
        }

        [HttpDelete]
        public JsonResult DeleteCategory(byte categoryId)
        {
            bool status = false;
            try
            {
                status = repository.DeleteCategory(categoryId);
            }
            catch (Exception ex)
            {

                status = false; ;
            }
            return Json(status);
        }
    }

}

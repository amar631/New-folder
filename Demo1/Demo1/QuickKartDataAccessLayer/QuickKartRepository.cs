using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using QuickKartDataAccessLayer.Models;
using Microsoft.Data.SqlClient;

namespace QuickKartDataAccessLayer
{
    public class QuickKartRepository
    {

        private QuickKartDBContext context;

        public QuickKartDBContext Context { get { return context; } }

        public QuickKartRepository(QuickKartDBContext dbContext)
        {
            context = dbContext;
        }
        public QuickKartRepository()
        {
            context = new QuickKartDBContext();
        }

        #region Library for Services Demo

        #region-To get all product details
        public List<Products> GetAllProducts()
        {
            List<Products> lstProducts = null;
            try
            {
                lstProducts = (from p in context.Products
                               orderby p.ProductId
                               select p).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                lstProducts = null;
            }
            return lstProducts;
        }
        #endregion

        #region-To get a product detail by using ProductId
        public Products GetProductById(string productId)
        {
            Products product = new Products();
            try
            {
                product = (from p in context.Products
                           where p.ProductId.Equals(productId)
                           select p).FirstOrDefault();
            }
            catch (Exception)
            {
                product = null;
            }
            return product;
        }
        #endregion

        #region- To generate new product id
        public string GenerateNewProductId()
        {
            string productId;
            try
            {
                productId = (from p in context.Products
                             select
  QuickKartDBContext.ufn_GenerateNewProductId()).FirstOrDefault();
            }
            catch (Exception)
            {
                productId = null;
            }
            return productId;
        }
        #endregion

        #region- To add a new product using Params
        public bool AddProduct(string productName, byte categoryId, decimal price, int quantityAvailable, out string productId)
        {
            bool status = false;
            productId = null;
            try
            {
                Products prodObj = new Products();
                prodObj.ProductId = GenerateNewProductId();
                productId = prodObj.ProductId;
                prodObj.ProductName = productName;
                prodObj.CategoryId = categoryId;
                prodObj.Price = price;
                prodObj.QuantityAvailable = quantityAvailable;
                context.Products.Add(prodObj);
                context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                productId = null;
                status = false;

            }
            return status;
        }
        #endregion

        #region- To add a new product Using Entity Models 
        public bool AddProduct(Products product)
        {
            bool status = false;
            try
            {
                product.ProductId = GenerateNewProductId();
                context.Products.Add(product);
               context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {

                status = false;

            }
            return status;
        }

        #endregion

        #region- To update the existing product details using Entity Models
        public bool UpdateProduct(Products products)
        {
            bool status = false;
            try
            {
                var product = (from prdct in context.Products
                               where prdct.ProductId == products.ProductId
                               select prdct).FirstOrDefault<Products>();
                if (product != null)
                {
                    product.ProductId = products.ProductId;
                    product.ProductName = products.ProductName;
                    product.Price = products.Price;
                    product.QuantityAvailable = products.QuantityAvailable;
                    product.CategoryId = products.CategoryId;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region-To delete a existing product
        public bool DeleteProduct(string prodId)
        {
            bool status = false;
            try
            {
                var product = (from prdct in context.Products
                               where prdct.ProductId == prodId
                               select prdct).FirstOrDefault<Products>();
                context.Products.Remove(product);
                context.SaveChanges();
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region Library for Services Exercise

        #region-To get all category details
        public List<Categories> GetAllCategories()
        {
            List<Categories> lstCategories = null;
            try
            {
                lstCategories = (from c in context.Categories
                                 orderby c.CategoryId ascending
                                 select c).ToList<Categories>();
            }
            catch (Exception)
            {
                lstCategories = null;
            }
            return lstCategories;
        }
        #endregion

        #region get a category detail by using CategoryId
        public Categories GetCategoryById(byte categoryId)
        {
            Categories categories = new Categories();
            try
            {

                categories = (from c in context.Categories
                              where c.CategoryId.Equals(categoryId)
                              select c).FirstOrDefault();
            }
            catch (Exception)
            {
                categories = null;
            }
            return categories;
        }
        #endregion

        #region-To generate new category id
        public byte GenerateNewCategoryId()
        {
            byte categoryId;
            try
            {
                var newCategoryId = (from p in context.Categories select QuickKartDBContext.ufn_GenerateNewCategoryId()).FirstOrDefault();
                categoryId = Convert.ToByte(newCategoryId);
            }
            catch (Exception ex)
            {
                categoryId = 0;
            }
            return categoryId;

        }
        #endregion

        #region-To add a new category using EF Models
        public bool AddCategory(Categories category)
        {
            bool status = false;
            try
            {
                context.Categories.Add(category);
                context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region-To add a new category using Params
        public bool AddCategory(string categoryName)
        {
            bool status = false;
            try
            {
                Categories catObj = new Categories();
                catObj.CategoryName = categoryName;
                context.Categories.Add(catObj);
                context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion


        public int AddCategoryDetailsUsingUSP(string categoryName, out byte categoryId)
        {
            int noOfRowsAffected = 0;
            categoryId = 0;
            int returnResult = 0;

            SqlParameter prmCategoryName = new SqlParameter("@CategoryName",categoryName);
            SqlParameter prmCategoryId = new SqlParameter("@CategoryId",System.Data.SqlDbType.TinyInt);
            prmCategoryId.Direction = System.Data.ParameterDirection.Output;
            SqlParameter prmReturnResult = new SqlParameter("@ReturnValue",System.Data.SqlDbType.Int);
            prmReturnResult.Direction = System.Data.ParameterDirection.Output;
            try
            {
                noOfRowsAffected=context.Database.ExecuteSqlRaw("EXEC @ReturnValue=usp_AddCategory @CategoryName,@CategoryId OUT",prmReturnResult,prmCategoryName,prmCategoryId);
                returnResult = Convert.ToInt32(prmReturnResult.Value);
                categoryId = Convert.ToByte(prmCategoryId.Value);
            }
            catch (Exception ex)
            {
                categoryId = 0;
                noOfRowsAffected = -1;
                returnResult = -99;
            }
            return returnResult ;
        }


        #region-To update the existing category details 
        public bool UpdateCategory(Categories category)
        {
            bool status = false;
            try
            {
                var catObj = (from ctgry in context.Categories
                              where ctgry.CategoryId == category.CategoryId
                              select ctgry).FirstOrDefault<Categories>();

                if (category != null)
                {
                    catObj.CategoryName = category.CategoryName;
                      context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        #endregion

        #region-To delete a existing category
        public bool DeleteCategory(byte categID)
        {
            bool status = false;
            try
            {
                var category = (from ctgry in context.Categories
                                where ctgry.CategoryId == categID
                                select ctgry).FirstOrDefault<Categories>();
                context.Categories.Remove(category);
                 context.SaveChanges();
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        #endregion
        #endregion
    }
}
#endregion
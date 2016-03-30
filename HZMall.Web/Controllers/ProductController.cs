using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HZMall.IServices.Products;
using HZMall.Service.Common;

namespace HZMall.Web.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class ProductController : BaseController
    {

        private ICategoryService _categoryService;

        public ProductController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        /// <summary>
        /// 产品类型列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryList()
        {
            var langId = CultureHelper.GetLanguageID();
            var cateInfo = _categoryService.GetCategoryByGrade(1, langId).Data;
            return View(cateInfo);
        }

    }
}
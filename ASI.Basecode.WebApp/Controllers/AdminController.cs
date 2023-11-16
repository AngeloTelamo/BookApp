using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IBookMasterService _bookMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
        //private readonly IBookService _bookService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="bookMasterService"></param>
        /// <param name="mapper"></param>
        public AdminController(IHttpContextAccessor httpContextAccessor, 
                    ILoggerFactory loggerFactory, 
                    IConfiguration configuration, 
                    IBookMasterService bookMasterService,
                    IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
          _bookMasterService = bookMasterService;
          this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminBookAdd() {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminBookAdd(BookMasterViewModel model, IFormFile BookImageFile)
        {
            try
            {
                if (BookImageFile != null && BookImageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;  //embedded function if kailangan mag hosting 
                    string fileName = Path.GetFileNameWithoutExtension(BookImageFile.FileName);
                    string extension = Path.GetExtension(model.BookImageFile.FileName);

                    model.BookImage = fileName + DateTime.Now.ToString("yymmssff") + extension;

                    string path = Path.Combine(wwwRootPath + "/books/", fileName); 

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.BookImageFile.CopyToAsync(fileStream);
                    }
                }

                // Insert book into database
                _bookMasterService.AddBook(model);
                return RedirectToAction("AdminBookAdd", "Admin");
            }
            catch (InvalidDataException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }

            return View(model);
        }




        public IActionResult AdminBookList()
        {
            var dataList = _bookMasterService.GetBookList(null); // naa sa BookmasterService ang logic sa list 
            return View("AdminBookList", dataList);
        }

        [HttpGet]
        public IActionResult AdminBookUpdate(string bId)
        {
            var model = _bookMasterService.GetBook(bId); // refers in BookmasterService
            return View(model);
        }

        [HttpPost]
        public IActionResult AdminBookUpdate(BookMasterEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)    
                {
                    _bookMasterService.UpdateBook(model); // refers in BookmasterService
                    TempData["SuccessMessage"] = "Saved!";
                }

            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }

        [HttpGet]
        public IActionResult RemoveBook(string bId) //reference object sa BId kung naa ba sa repository 
        {
            try
            {
                _bookMasterService.DeleteBook(bId); // refers in BookmasterService
                return AdminBookList();
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();
        }

        [HttpPost]
        public IActionResult SearchBook(BookMasterListViewModel model)
        {
            var dataList = _bookMasterService.GetBookList(model);   // makasearch siya sa base sa fields sa views/admin/adminBookList
            return View("AdminBookList", dataList);
        }
    }
}

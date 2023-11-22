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

    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IUserService _userService;
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
        public HomeController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IUserService userService,
                              IBookMasterService bookMasterService,
                              IMapper mapper,
                              IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookMasterService = bookMasterService;
            _userService = userService;
            this._webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Returns Home View.
        /// </summary>
        /// <returns> Home View </returns>
        public IActionResult Dashboard()
        {
            var dataList = _bookMasterService.GetBookList(null); // naa sa BookmasterService ang logic sa list 
            return View("Dashboard", dataList);
        }
        public IActionResult BookList()
        {
            var dataList = _bookMasterService.GetBookList(null);
            return View("BookList", dataList);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BookAdd()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> BookAdd(BookMasterViewModel model, IFormFile BookImageFile)
        {
            try
            {
                if (BookImageFile != null && BookImageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;  //embedded function if kailangan mag hosting 
                    string fileName = Path.GetFileNameWithoutExtension(BookImageFile.FileName);
                    string extension = Path.GetExtension(model.BookImageFile.FileName);

                    model.BookImage = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;

                    string path = Path.Combine(wwwRootPath + "/books", fileName);

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
    }
}

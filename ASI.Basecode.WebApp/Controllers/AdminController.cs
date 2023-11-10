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
using System;
using System.IO;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IBookMasterService _bookMasterService;
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
                    IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
          _bookMasterService = bookMasterService;

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
        public IActionResult AdminBookAdd(BookMasterViewModel model)
        {
            try
            {
                _bookMasterService.AddBook(model);
                return RedirectToAction("AdminBookAdd", "Admin");
            }
            catch(InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return View();

        }

        public IActionResult AdminBookList()
        {
            var dataList = _bookMasterService.GetBookList(null);
            return View("AdminBookList", dataList);
        }

        [HttpGet]
        public IActionResult Delete(string bId)
        {
            try
            {
                _bookMasterService.RemoveBook(bId);
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
    }
}

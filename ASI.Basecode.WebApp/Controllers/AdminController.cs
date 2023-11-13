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
using static ASI.Basecode.WebApp.Controllers.HomeController;
using static ASI.Basecode.WebApp.Controllers.AdminController;
using ASI.Basecode.Data.Models;
using System.Collections.Generic;
using ASI.Basecode.WebApp.Models;
using System.Threading.Tasks;

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
                    IMapper mapper): base(httpContextAccessor, loggerFactory, configuration, mapper)
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
        public IActionResult AdminBookUpdate(string bId)
        {
            var model = _bookMasterService.GetBook(bId);
            return View(model);
        }

        [HttpPost]
        public IActionResult AdminBookUpdate(BookMasterEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _bookMasterService.UpdateBook(model);
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
        public IActionResult RemoveBook(string bId)
        {
            try
            {
                _bookMasterService.DeleteBook(bId);
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
            var dataList = _bookMasterService.GetBookList(model);
            return View("AdminBookList", dataList);
        }
    }
}

using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using ASI.Basecode.Services.ServiceModels;

namespace ASI.Basecode.WebAppAdmin.Controllers
{
    public class BookController : ControllerBase<HomeController>
    {
        private readonly IBookService _bookService;

        public BookController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IBookService bookService, // Change the parameter type to IBookService
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookService = bookService; // Assign the book service in the constructor.
        }

        public IActionResult Addbooks()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Addbooks(BookViewModel model)
        {
            try
            {
                _bookService.AddBooks(model); // Modify the method to add a single book
                return RedirectToAction("Addbooks", "Account");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred."; // Modify the error message as needed
            }
            return View();
        }
    }
}

using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using ASI.Basecode.Services.Services;

namespace ASI.Basecode.WebApp.Controllers
{
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IReviewService _reviewService;
        private readonly IBookGenreMasterService _bookGenreMasterService;
        private readonly IBookMasterService _bookMasterService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>
        public HomeController(IHttpContextAccessor httpContextAccessor,
                              ILoggerFactory loggerFactory,
                              IConfiguration configuration,
                              IReviewService reviewService,
                              IBookMasterService bookMasterService,
                              IBookGenreMasterService bookGenreMasterService,
                              IWebHostEnvironment webHostEnvironment,
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _reviewService = reviewService;
            _bookGenreMasterService = bookGenreMasterService;
            _bookMasterService = bookMasterService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult BookReviews(int bookId)
        {
            try
            {
                var bookDetails = _bookMasterService.GetBookById(bookId);
                var fileContent = _bookMasterService.GetBookFileContent(bookId);

                if (bookDetails != null)
                {
                    var bookReviewsModel = new BookReviewsModel
                    {
                        BookMaster = bookDetails,
                        BookId = bookId,
                    };

                    return View(bookReviewsModel);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (InvalidDataException ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ReadBooks(int bookId)
        {
            var fileContent = _bookMasterService.GetBookFileContent(bookId);

            if (fileContent != null)
            {
                return View("ReadBooks", (object)fileContent);
            }
            else
            {
                return Content("File not found or empty.", "text/plain");
            }
        }

        [HttpPost]
        public IActionResult BookReviews(BookReviewsModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {                  
                    _reviewService.AddReviews(reviewModel);
                    return RedirectToAction("BookReviews");
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the review. Please try again.");
                }
            }
            return View();  
        }


        [HttpPost]
        public IActionResult Search(BookMasterListViewModel model)
        {
            var dataList = _bookMasterService.GetBookList(model);   // makasearch siya sa base sa fields sa views/admin/adminBookList
            return View("Index", dataList);
        }

        public IActionResult Index()
        {
            var dataList = _bookMasterService.GetBookList(null); // naa sa BookmasterService ang logic sa list 
            return View("Index", dataList);
        }

        public IActionResult Discover()
        {
            var dataList = _bookGenreMasterService.GetGenreList(null); // press genre then display its books should add if genreID or genreName
            return View("Discover", dataList);
        }

        [HttpPost]
        public IActionResult Discover(BookGenreList model)
        {
            var dataList = _bookGenreMasterService.GetGenreList(model); // search any books no genreid/name included
            return View("Discover", dataList);
        }

        public IActionResult Dashboard()
        {
            var newBooksModel = _bookMasterService.GetNewBooks(null); // need GetNewBooks and GetTopBooks and GetBookList
            return View("Dashboard", newBooksModel);
        }

        public IActionResult TopBooks()
       {
            var topBooksModel = _bookMasterService.GetTopBooks(null);
            return View("TopBooks", topBooksModel);  
       }

        public IActionResult NewBooks()
        {
            var newBooksModel = _bookMasterService.GetNewBooks(null);
            return View("NewBooks", newBooksModel);
        }

    }
}

using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.IO;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace ASI.Basecode.WebApp.Controllers
{
    public class HomeController : ControllerBase<HomeController>
    {
        private readonly IReviewService _reviewService;
        private readonly IBookMasterService _bookMasterService;

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
                              IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _reviewService = reviewService;
            _bookMasterService = bookMasterService;
        }

        [HttpGet]
        public IActionResult BookReviews(int bookId)
        {
            try
            {
                var bookDetails = _bookMasterService.GetBookById(bookId);
                var bookReviewsModel = new BookReviewsModel
                {
                    BookMaster = bookDetails,
                    BookId = bookId
                };
                return View(bookReviewsModel);
            }
            catch (InvalidDataException ex)
            {
                return RedirectToAction("Index");  //if it works it works ahahaha
            }
        }

        [HttpPost]
        public IActionResult BookReviews(BookReviewsModel reviewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   // reviewModel.Reviews.BookId = reviewModel.BookMaster.BookId;
                    _reviewService.AddReviews(reviewModel);
                    return RedirectToAction("BookReviews");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the review. Please try again.");
                }
            }
            return View();  // This might be the case where ModelState is not valid, and it returns the view without executing the review submission.
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

    }
}

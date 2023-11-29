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
using System.Net;
using Humanizer.Localisation;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminController : ControllerBase<AdminController>
    {
        private readonly IBookMasterService _bookMasterService;
        private readonly IBookGenreMasterService _bookGenreMasterService;
        private readonly IReviewService _reviewService;
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
                    IBookGenreMasterService bookGenreMasterService,
                    IReviewService reviewService,
                    IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookMasterService = bookMasterService;
            _bookGenreMasterService = bookGenreMasterService; 
            _reviewService = reviewService;
          this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminBookAdd(int genreId)
        {
            try
            {
                var genreModel = new BookGenres
                {
                    BookGenreId = genreId
                };
                return View(genreModel);
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index", "Home"); 
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminBookAdd(BookGenres model, IFormFile BookImageFile, IFormFile BookFileText)
        {
            try
            {
                if (BookImageFile != null && BookImageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(BookImageFile.FileName);
                    string extension = Path.GetExtension(BookImageFile.FileName);

                    model.Master.BookImage = fileName = fileName + DateTime.Now.ToString("yymmssff") + extension;

                    string path = Path.Combine(wwwRootPath + "/books/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await BookImageFile.CopyToAsync(fileStream);
                    }
                }
                
                if (BookFileText != null && BookFileText.Length > 0)
                {
                    string appDataPath = Path.Combine(_webHostEnvironment.WebRootPath, "appData");
                    string textFileName = Path.GetFileName(BookFileText.FileName);
                    string textFilePath = Path.Combine(appDataPath, textFileName);

                    using (var fileStream = new FileStream(textFilePath, FileMode.Create))
                    {
                        await BookFileText.CopyToAsync(fileStream);
                    }
                    model.Master.BookFilePath = textFilePath;
                    model.Master.BookContext = textFilePath;    
                }

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

        [HttpGet]
        public IActionResult BookGenreAdd(int genreId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult BookGenreAdd(BookGenres model)
        {
            try
            {
                _bookGenreMasterService.AddGenre(model);
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("BookGenreAdd", "Admin");
        }

        [HttpGet]
        public IActionResult RemoveGenre(int genreId)
        {
            try
            {
                _bookGenreMasterService.DeleteGenre(genreId);
                return RedirectToAction("BookGenreList");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("BookGenreList");
        }

        public ActionResult BookContents(int bookId)
        {
            var fileContent = _bookMasterService.GetBookFileContent(bookId);

            if (fileContent != null)
            {
                return View("BookContents", (object)fileContent);
            }
            else
            {
                return Content("File not found or empty.", "text/plain");
            }
        }

        public IActionResult AdminBookList()
        {
            var dataList = _bookMasterService.GetBookList(null); 
            return View("AdminBookList", dataList);
        }

        [HttpGet]
        public IActionResult AdminBookUpdate(int bookId)
        {
            var model = _bookMasterService.GetBooks(bookId); 
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
                    return RedirectToAction("AdminBookList");
                }
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the book.";
            }
            return View("AdminBookUpdate", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewBooks(int genreId, string genreName)
        {
            try
            {
                var booksForGenre = _bookMasterService.GetBooksForGenre(genreId);
                var model = new BookMasterListViewModel
                {
                    BookList = booksForGenre.ToList(),
                    Filters = new BookMasterListViewModel.BookListFilterModel
                    {
                        GenreId = genreId,
                        GenreName = genreName
                    }
                };
                return View("ViewBooks", model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult RemoveBook(int bookId)  
        {
            try
            {
                _bookMasterService.DeleteBook(bookId); 
                return RedirectToAction("AdminBookList");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }
            return RedirectToAction("AdminBookList");
        }

        [HttpPost]
        public IActionResult SearchBook(BookMasterListViewModel model)
        {
            var dataList = _bookMasterService.GetBookList(model);   
            return View("AdminBookList", dataList);
        }
        
        public IActionResult BookGenreList()
        {
            var dataList = _bookGenreMasterService.GetGenreList(null); 
            return View("BookGenreList", dataList);
        }

        [HttpPost]
        public IActionResult SearchGenre(BookGenreList model)
        {
            var dataList = _bookGenreMasterService.GetGenreList(model);
            return View("BookGenreList", dataList);
        }

        [HttpPost]
        public IActionResult SearchReview(ReviewList model)
        {
            var dataList = _reviewService.GetReviewList(model);  
            return View("BookReviewList", dataList);
        }

        [HttpGet]
        public IActionResult RemoveReview(int reviewId)
        {
            try
            {
                _reviewService.DeleteReview(reviewId);
                return RedirectToAction("BookReviewList");
            }
            catch (InvalidDataException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = Resources.Messages.Errors.ServerError;
            }

            return RedirectToAction("AdminBookList");
        }
        public IActionResult BookReviewList()
        {
            var dataList = _reviewService.GetReviewList(null); 
            return View("BookReviewList", dataList);
        }
    }
}

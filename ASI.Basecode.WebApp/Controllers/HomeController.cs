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
                              IBookMasterService bookMasterService,
                              IMapper mapper,
                              IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            _bookMasterService = bookMasterService;
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

        public IActionResult Discover()
        {            
            return View();
        }

        public IActionResult BookList()
        {
            var dataList = _bookMasterService.GetBookList(null);
            return View("BookList", dataList);
        }
    }
}

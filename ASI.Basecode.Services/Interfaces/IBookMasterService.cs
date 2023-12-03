﻿using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookMasterService
    {
        BookMasterListViewModel GetBookList(BookMasterListViewModel model);
        BookMasterListViewModel GetTopBooks(BookMasterListViewModel model);
        BookMasterListViewModel GetNewBooks(BookMasterListViewModel model);
        BookMasterEditViewModel GetBooks(int bookId);
        BookMasterViewModel GetBookById(int bookId);
        IEnumerable<BookMasterViewModel> GetBooksForGenre(int genreId);
        void AddBook(BookGenres model);
        void UpdateBook(BookMasterEditViewModel model);
        void DeleteBook(int bookId);
        string GetBookFileContent(int bookId);
      
    }
}
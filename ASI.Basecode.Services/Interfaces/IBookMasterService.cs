using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IBookMasterService
    {
        BookMasterListViewModel GetBookList(BookMasterListViewModel model);
        BookMasterEditViewModel GetBooks(int bookId);
        BookMasterViewModel GetBookById(int bookId);
        void AddBook(BookMasterViewModel model);
        void UpdateBook(BookMasterEditViewModel model);
        void DeleteBook(int bookId);
    }
}
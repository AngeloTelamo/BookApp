using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.AspNetCore.Http;

namespace ASI.Basecode.Services.Services
{
    public class BookMasterService : IBookMasterService
    {
        private readonly IBookMasterRepository _repository;
        private readonly IMapper _mapper;
    


        public BookMasterService(IBookMasterRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
           
        }

        public void AddBook(BookMasterViewModel model)
        {
            var book = new BookMaster();

            if (!_repository.BookExists(model.BId))
            {
                _mapper.Map(model, book);
                book.BookAdded = DateTime.Now;
                book.CreatedBy = System.Environment.UserName;

                if (!string.IsNullOrEmpty(model.BookImage)) //validation if it is empty
                {
                    book.BookImage = model.BookImage; // Store the file path in the BookImage property in BookMasterViewModel
                }
                _repository.AddBook(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
        }
        public BookMasterListViewModel GetBookList(BookMasterListViewModel model)
        {
            BookMasterListViewModel listModel = new BookMasterListViewModel();

            var queryData = _repository.GetBooks(); 

            if (model != null)
            {
                queryData = queryData.Where(x =>
                    (string.IsNullOrEmpty(model.Filters.BId) || x.BId.ToLower().Contains(model.Filters.BId.ToLower()))
                    && (string.IsNullOrEmpty(model.Filters.BookTitle) || x.BookTitle.ToLower().Contains(model.Filters.BookTitle.ToLower()))
                    && (string.IsNullOrEmpty(model.Filters.BookAuthor) || x.BookAuthor.ToLower().Contains(model.Filters.BookAuthor.ToLower()))
                    && (string.IsNullOrEmpty(model.Filters.BookDes) || x.BookDes.ToLower().Contains(model.Filters.BookDes.ToLower())));
            }

            listModel.BookList = queryData
                .Select(x => new BookMasterViewModel
                {
                    BId = x.BId,
                    BookTitle = x.BookTitle,
                    BookAuthor = x.BookAuthor,
                    BookDes = x.BookDes,
                   
                }).ToList();

            listModel.Filters = model?.Filters ?? new BookMasterListViewModel.BookListFilterModel();

            return listModel;
        }


        public void UpdateBook(BookMasterEditViewModel model)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BId == model.BId)
                                 .FirstOrDefault();

            if (book != null)
            {
                _mapper.Map(model, book);

                book.BookAdded = DateTime.Now;
                _repository.UpdateBooks(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
        }

        public BookMasterEditViewModel GetBook(string bId)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BId == bId)
                                 .Select(x => new BookMasterEditViewModel
                                 {
                                     BId = x.BId,
                                 })
                                 .FirstOrDefault();

            if (book == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }

            return book;
        }

        public void DeleteBook(string bId)
        {
            var book = _repository.GetBooks().AsNoTracking()  
                                 .Where(x => x.BId == bId)
                                 .FirstOrDefault();

            if (book != null)
            {
                _repository.DeleteBooks(book); //refers sa repository or IBookMasterRepository
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserNotExists);
            }
        }
    }

}

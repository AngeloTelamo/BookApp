using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Services
{
    public class BookService: IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void AddBooks(BookViewModel model)
        {
            var book = new Book();

            if (!_repository.BookExists(book.BookId))
            {

                _mapper.Map(model, book);
                
                book.CreatedTime = DateTime.Now;
                book.UpdatedTime = DateTime.Now;
                
                
                book.CreatedBy = Environment.UserName; // Use Environment.UserName to get the current user
                book.UpdatedBy = Environment.UserName;

                _repository.AddBooks(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BooksExists);
            }
        }


        public void UpdateBooks(BookEditViewModel model)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BookId == model.BookId)
                                 .FirstOrDefault();

            if (book != null)
            {
                _mapper.Map(model, book);

                book.UpdatedTime = DateTime.Now;
                book.UpdatedBy = System.Environment.UserName;

                _repository.UpdateBooks(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserNotExists);
            }
            //throw new NotImplementedException();
        }

        public void DeleteBooks(string bookId)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BookId == bookId)
                                 .FirstOrDefault();

            if (book != null)
            {
                _repository.DeleteBooks(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExist);
            }
            //throw new NotImplementedException();
        }

        public BookEditViewModel GetBook(string bookId)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BookId == bookId)
                                 .Select(x => new BookEditViewModel
                                 {
                                     BookId = x.BookId,
                                     BookTitle = x.BookTitle,
                                     BookGenre = x.BookGenre,
                                     BookAuthor = x.BookAuthor,
                                     BookSeries = x.BookSeries
                                 })
                                 .FirstOrDefault();

            if (book == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExist);
            }

            return book;
            // throw new NotImplementedException();
        }

            public BookListViewModel GetBookList(BookListViewModel model)
        {

            BookListViewModel listModel = new BookListViewModel();

            var queryData = _repository.GetBooks(); // DB is not accessed yet

            if (model != null)
            {
                queryData = queryData.Where(x =>
                                       (string.IsNullOrEmpty(model.Filters.BookId) || x.BookId.ToLower().Contains(model.Filters.BookId.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookTitle) || x.BookTitle.ToLower().Contains(model.Filters.BookTitle.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookAuthor) || x.BookAuthor.ToLower().Contains(model.Filters.BookAuthor.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookGenre) || x.BookGenre.ToLower().Contains(model.Filters.BookGenre.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookSeries) || x.BookSeries.ToLower().Contains(model.Filters.BookSeries.ToLower())));

                // DB is still not accessed yet
            }

            listModel.BookList = queryData
                                .Select(x => new BookViewModel
                                {
                                    BookId = x.BookId,
                                    BookTitle = x.BookTitle,
                                    BookAuthor = x.BookAuthor,
                                    BookGenre = x.BookGenre,
                                    BookSeries = x.BookSeries,
                                }).ToList(); // Data is now retrieved from DB

            listModel.Filters = model?.Filters ?? new BookListViewModel.BookListFilterModel();

            return listModel;
        }
        //throw new NotImplementedException();
    }
}


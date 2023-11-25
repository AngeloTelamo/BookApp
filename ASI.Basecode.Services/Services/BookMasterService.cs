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

            if (!_repository.BookExists(model.BookId))
            {
                book.BookId = model.BookId;
                book.BookTitle = model.BookTitle;
                book.BookAuthor = model.BookAuthor;
                book.BookGenre = model.BookGenre;   
                book.BookSynopsis = model.BookSynopsis;
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

            if (model != null && model.Filters != null)
            {
                if (!string.IsNullOrEmpty(model.Filters.SearchTerm))
                {
                    queryData = queryData.Where(x =>
                        x.BookId.ToString().Contains(model.Filters.SearchTerm) ||
                        x.BookTitle.ToLower().Contains(model.Filters.SearchTerm.ToLower()) ||
                        x.BookAuthor.ToLower().Contains(model.Filters.SearchTerm.ToLower()) ||
                        x.BookGenre.ToLower().Contains(model.Filters.SearchTerm.ToLower())
                    );
                }
                else
                {
                    queryData = queryData.Where(x =>
                        (model.Filters.BookId == 0 || x.BookId == model.Filters.BookId)
                        && (string.IsNullOrEmpty(model.Filters.BookTitle) || x.BookTitle.ToLower().Contains(model.Filters.BookTitle.ToLower()))
                        && (string.IsNullOrEmpty(model.Filters.BookAuthor) || x.BookAuthor.ToLower().Contains(model.Filters.BookAuthor.ToLower()))
                        && (string.IsNullOrEmpty(model.Filters.BookGenre) || x.BookGenre.ToLower().Contains(model.Filters.BookGenre.ToLower()))
                    );
                }
            }

            listModel.BookList = queryData
                .Select(x => new BookMasterViewModel
                {
                    BookId = x.BookId,
                    BookTitle = x.BookTitle,
                    BookAuthor = x.BookAuthor,
                    BookGenre = x.BookGenre,
                    BookImage = x.BookImage
                }).ToList();

            listModel.Filters = model?.Filters ?? new BookMasterListViewModel.BookListFilterModel();

            return listModel;
        }

        public void UpdateBook(BookMasterEditViewModel model)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                  .Where(x => x.BookId == model.BookId)
                                  .FirstOrDefault();

            if (book != null)
            {
                book.BookAuthor = model.BookAuthor;
                book.BookTitle = model.BookTitle;
                book.BookId = model.BookId;
                book.BookSynopsis = model.BookSynopsis;
                book.BookGenre = model.BookGenre;
                _repository.UpdateBooks(book);
            }
            else
            {
                throw new InvalidDataException("Book not found.");
            }
        }

         public BookMasterEditViewModel GetBooks(int bookId)
        {
            var book = _repository.GetBooks().AsNoTracking()
                                 .Where(x => x.BookId == bookId)
                                 .Select(x => new BookMasterEditViewModel
                                 {
                                     BookId = x.BookId,
                                     BookAuthor = x.BookAuthor,
                                     BookGenre = x.BookGenre,
                                     BookTitle = x.BookTitle,
                                     BookSynopsis = x.BookSynopsis,
                                     BookImage = x.BookImage
                                 })
                                 .FirstOrDefault();

            if (book == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }

            return book;
        } 

        public BookMasterViewModel GetBookById(int bookId)
        {
            var book = _repository.GetBooks()
                                 .Where(x => x.BookId == bookId)
                                 .Select(x => new BookMasterViewModel
                                 {
                                     BookTitle = x.BookTitle,
                                     BookAuthor = x.BookAuthor,
                                     BookGenre = x.BookGenre,
                                     BookImage = x.BookImage,
                                     BookSynopsis = x.BookSynopsis,
                                 })
                                 .FirstOrDefault();

            if (book == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }

            return book;
        }

        public void DeleteBook(int bookId)
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
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
        }
    }

 }



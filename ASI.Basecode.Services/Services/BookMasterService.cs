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
        private readonly IBookGenreMasterRepository _repositoryGenre;
        private readonly IMapper _mapper;

        public BookMasterService(IBookMasterRepository repository, IMapper mapper, IBookGenreMasterRepository repositoryGenre)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryGenre = repositoryGenre;
        }

        public void AddBook(BookGenres model)
        {
            var book = new BookMaster();

            if (!_repository.BookExists(model.Master.BookId))
            {

                book.BookId = model.Master.BookId;
                book.BookTitle = model.Master.BookTitle;
                book.BookAuthor = model.Master.BookAuthor;
                book.BookGenreId = model.BookGenreId;  ///bookmaster <- bookgenre
                book.BookSynopsis = model.Master.BookSynopsis;
                book.BookAdded = DateTime.Now;
                book.CreatedBy = System.Environment.UserName;

                if (!string.IsNullOrEmpty(model.Master.BookImage)) //validation if it is empty
                {
                    book.BookImage = model.Master.BookImage; // Store the file path in the BookImage property in BookMasterViewModel
                }
                book.BookFile = model.Master.BookContext;
                _repository.AddBook(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
        }
        public string GetBookFileContent(int bookId)
        {
            var book = _repository.GetBookById(bookId);

            if (book != null && !string.IsNullOrEmpty(book.BookFile))
            {
                string fileContent = File.ReadAllText(book.BookFile);
                return fileContent;
            }
            return null;
        }

        public BookMasterListViewModel GetBookList(BookMasterListViewModel model)
        {
            BookMasterListViewModel listModel = new BookMasterListViewModel();

            var queryData = _repository.GetBooks();

            if (model != null && model.Filters != null)
            {
                if (!string.IsNullOrEmpty(model.Filters.SearchTerm))
                {
                    queryData = queryData
                        .Include(x => x.genreMaster) // Assuming "GenreMaster" is the navigation property in your Book entity
                        .Where(x =>
                            x.BookId.ToString().Contains(model.Filters.SearchTerm) ||
                            x.BookTitle.ToLower().Contains(model.Filters.SearchTerm.ToLower()) ||
                            x.BookAuthor.ToLower().Contains(model.Filters.SearchTerm.ToLower()) ||
                            x.genreMaster.GenreName.ToLower().Contains(model.Filters.SearchTerm.ToLower())
                        );
                }
                else
                {
                    queryData = queryData
                        .Include(x => x.genreMaster) // Assuming "GenreMaster" is the navigation property in your Book entity
                        .Where(x =>
                            (model.Filters.BookId == 0 || x.BookId == model.Filters.BookId)
                            && (string.IsNullOrEmpty(model.Filters.BookTitle) || x.BookTitle.ToLower().Contains(model.Filters.BookTitle.ToLower()))
                            && (string.IsNullOrEmpty(model.Filters.BookAuthor) || x.BookAuthor.ToLower().Contains(model.Filters.BookAuthor.ToLower()))
                            && (string.IsNullOrEmpty(model.Filters.GenreName) || x.genreMaster.GenreName.ToLower().Contains(model.Filters.GenreName.ToLower()))
                        );
                }
                queryData = queryData
                       .Include(x => x.Reviews)
                        .Where(x => x.Reviews.Any());
            }

               listModel.BookList = queryData
                  .Select(x => new BookMasterViewModel
                  {
                      BookId = x.BookId,
                      BookTitle = x.BookTitle,
                      BookAuthor = x.BookAuthor,
                      BookImage = x.BookImage,
                      BookGenreName = x.genreMaster.GenreName,
                      AverageRating = x.Reviews.Average(r => r.ReviewRatings),
                      ReviewCount = x.Reviews.Count(),
                      IsTopBook = x.Reviews.Count() > 5 && x.Reviews.Average(r => r.ReviewRatings) > 4.5 
                  })
                  .OrderByDescending(x => x.IsTopBook)
                  .ThenByDescending(x => x.AverageRating)
                  .ThenByDescending(x => x.ReviewCount)
                  .ToList();

            listModel.Filters = model?.Filters ?? new BookMasterListViewModel.BookListFilterModel();
                
            return listModel;
        }

        public BookMasterListViewModel GetTopBooks(BookMasterListViewModel model)
        {

            BookMasterListViewModel toplistModel = new BookMasterListViewModel();

            var queryData = _repository.GetBooks();

            var topBooksQuery = queryData
                .Include(x => x.genreMaster)
                .Include(x => x.Reviews)
                .Where(x => x.Reviews.Any())
                .OrderByDescending(x => x.Reviews.Count() > 5 && x.Reviews.Average(r => r.ReviewRatings) > 4.5)
                .ThenByDescending(x => x.Reviews.Average(r => r.ReviewRatings))
                .ThenByDescending(x => x.Reviews.Count())
                .Take(5);

            toplistModel.BookList = topBooksQuery
                .Select(x => new BookMasterViewModel
                {
                    BookId = x.BookId,
                    BookTitle = x.BookTitle,
                    BookAuthor = x.BookAuthor,
                    BookImage = x.BookImage,
                    BookGenreName = x.genreMaster.GenreName,
                    AverageRating = x.Reviews.Average(r => r.ReviewRatings),
                    ReviewCount = x.Reviews.Count(),
                    IsTopBook = x.Reviews.Count() > 5 && x.Reviews.Average(r => r.ReviewRatings) > 4.5
                })
                .ToList();


            toplistModel.Filters = model?.Filters ?? new BookMasterListViewModel.BookListFilterModel();

            return toplistModel;
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
                                     BookGenreName = x.genreMaster.GenreName,
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
            var book = _repository.GetBooks()
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
        public IEnumerable<BookMasterViewModel> GetBooksForGenre(int genreId)
        {
            var books = _repository.GetBooks()
                .Include(x => x.genreMaster)
                .Where(x => x.BookGenreId == genreId)
                .Select(x => new BookMasterViewModel
                {
                    BookId = x.BookId,
                    BookTitle = x.BookTitle,
                    BookAuthor = x.BookAuthor,
                    BookImage = x.BookImage,
                    BookSynopsis = x.BookSynopsis,
                    BookGenreName = x.genreMaster.GenreName
                })
                .ToList();
            return books;
        }

       

    }

}
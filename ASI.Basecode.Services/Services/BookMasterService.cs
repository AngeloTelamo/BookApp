using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ASI.Basecode.Resources.Constants.Enums;

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

            var queryData = _repository.GetBooks(); // DB is not accessed yet

            if (model != null)
            {
                queryData = queryData.Where(x =>
                                    (string.IsNullOrEmpty(model.Filters.BId) || x.BId.ToLower().Contains(model.Filters.BId.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookTitle) || x.BookTitle.ToLower().Contains(model.Filters.BookTitle.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookAuthor) || x.BookAuthor.ToLower().Contains(model.Filters.BookAuthor.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.BookDes) || x.BookDes.ToLower().Contains(model.Filters.BookDes.ToLower())));
                // DB is still not accessed yet
            }

            listModel.BookList = queryData
                                .Select(x => new BookMasterViewModel
                                {
                                    BId = x.BId,
                                    BookTitle = x.BookTitle,
                                    BookAuthor = x.BookAuthor,
                                    BookDes = x.BookDes
                                }).ToList(); // Data is now retrieved from DB

            listModel.Filters = model?.Filters ?? new BookMasterListViewModel.BookListFilterModel();

            return listModel;
        }
        
    }
}

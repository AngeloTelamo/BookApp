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
               //book.CreatedBy = System.Environment.UserName;

                _repository.AddBook(book);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
        }
    }
}

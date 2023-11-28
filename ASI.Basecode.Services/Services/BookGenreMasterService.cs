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
using ASI.Basecode.Data.Repositories;

namespace ASI.Basecode.Services.Services
{
    public class BookGenreMasterService : IBookGenreMasterService
    {
        private readonly IBookGenreMasterRepository _genreRepository;

        public BookGenreMasterService(IBookGenreMasterRepository genreRepository)
        {
            _genreRepository = genreRepository;

        }

        public void AddGenre(BookGenres genre)
        {
            var gen = new BookGenreMaster();

            gen.GenreId = genre.Genre.GenreId;
            gen.GenreName = genre.Genre.GenreName;
            _genreRepository.AddGenre(gen);
        }

        public BookGenreList GetGenreList(BookGenreList model)
        {
            BookGenreList listModel = new BookGenreList();

            var queryData = _genreRepository.GetGenre();

            if (model != null && model.Filters != null)
            {
                if (!string.IsNullOrEmpty(model.Filters.SearchTerm))
                {

                    queryData = queryData.Where(x =>
                        x.GenreId.ToString().Contains(model.Filters.SearchTerm) ||
                        x.GenreName.ToLower().Contains(model.Filters.SearchTerm.ToLower())
                    );
                }
                else
                {
                    queryData = queryData.Where(x =>
                        (model.Filters.GenreId == 0 || x.GenreId == model.Filters.GenreId)
                        && (string.IsNullOrEmpty(model.Filters.GenreName) || x.GenreName.ToLower().Contains(model.Filters.GenreName.ToLower()))
                    );
                }
            }
            listModel.GenreList = queryData
                .Select(x => new BookGenres
                {
                    BookGenreId = x.GenreId, 
                    Genre = new BookGenreModel
                    {
                        GenreId = x.GenreId,
                        GenreName = x.GenreName
                        
                    }
                }).ToList();

            listModel.Filters = model?.Filters ?? new BookGenreList.GenreListFilterModel();

            return listModel;
        }

        public BookGenres getGenreById(int genreId)
        {
            var genre = _genreRepository.GetGenre()
                     .Where(x => x.GenreId == genreId)
                         .Select(x => new BookGenres
                         {
                              BookGenreId = x.GenreId,  
                              Genre = new BookGenreModel
                              {
                                  GenreId = x.GenreId,
                                  GenreName = x.GenreName
                              }
                         })
                             .FirstOrDefault();

            if (genre == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.BookNotExists);
            }
            return genre;
        }

        public void DeleteGenre(int genreId)
        {
            var deleteGenre = _genreRepository.GetGenre()
                            .Where(x => x.GenreId == genreId)
                            .FirstOrDefault();

            if (deleteGenre != null)
            {
                _genreRepository.DeleteGenres(deleteGenre);
            }
            else
            {
                throw new InvalidDataException(ASI.Basecode.Resources.Messages.Errors.BookNotExists);
            }
        }
    }

}

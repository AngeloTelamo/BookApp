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
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public LoginResult AuthenticateUser(string userId, string password, ref User user)
        {
            user = new User();
            var passwordKey = PasswordManager.EncryptPassword(password);
            user = _repository.GetUsers().Where(x => x.Email == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.UserExists(model.Email))
            {
                _mapper.Map(model, user);
                user.Password = PasswordManager.EncryptPassword(model.Password);
                user.CreatedTime = DateTime.Now;
                user.UpdatedTime = DateTime.Now;
                user.CreatedBy = System.Environment.UserName;
                user.UpdatedBy = System.Environment.UserName;

                _repository.AddUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserExists);
            }
        }

        public void UpdateUser(UserEditViewModel model)
        {
            var user = _repository.GetUsers().AsNoTracking()
                                 .Where(x => x.Email == model.Email)
                                 .FirstOrDefault();

            if (user != null)
            {
                _mapper.Map(model, user);

                if (model.UpdatePasswordFlg)
                {
                    if (!string.IsNullOrWhiteSpace(model.Password))
                    {
                        user.Password = PasswordManager.EncryptPassword(model.Password);
                    }

                }

                user.UpdatedTime = DateTime.Now;
                user.UpdatedBy = System.Environment.UserName;

                _repository.UpdateUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserNotExists);
            }
        }

        public void DeleteUser(string email)
        {
            var user = _repository.GetUsers().AsNoTracking()
                                 .Where(x => x.Email == email)
                                 .FirstOrDefault();

            if (user != null)
            {
                _repository.DeleteUser(user);
            }
            else
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserNotExists);
            }
        }

        public UserEditViewModel GetUser(string email)
        {
            var user = _repository.GetUsers().AsNoTracking()
                                 .Where(x => x.Email == email)
                                 .Select(x => new UserEditViewModel
                                 {
                                     Email = x.Email,
                                     Name = x.Name
                                 })
                                 .FirstOrDefault();

            if (user == null)
            {
                throw new InvalidDataException(Resources.Messages.Errors.UserNotExists);
            }

            return user;
        }

        public UserListViewModel GetUserList(UserListViewModel model)
        {
            UserListViewModel listModel = new UserListViewModel();

            var queryData = _repository.GetUsers(); // DB is not accessed yet

            if (model != null)
            {
                queryData = queryData.Where(x =>
                                    (string.IsNullOrEmpty(model.Filters.Email) || x.Email.ToLower().Contains(model.Filters.Email.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.Name) || x.Name.ToLower().Contains(model.Filters.Name.ToLower())));

                // DB is still not accessed yet
            }

            listModel.UserList = queryData
                                .Select(x => new UserViewModel
                                {
                                    Email = x.Email,
                                    Name = x.Name
                                }).ToList(); // Data is now retrieved from DB

            listModel.Filters = model?.Filters ?? new UserListViewModel.UserListFilterModel();

            return listModel;
        }
    }
}

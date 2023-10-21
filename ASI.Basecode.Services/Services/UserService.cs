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
            user = _repository.GetUsers().Where(x => x.UserId == userId &&
                                                     x.Password == passwordKey).FirstOrDefault();

            return user != null ? LoginResult.Success : LoginResult.Failed;
        }

        public void AddUser(UserViewModel model)
        {
            var user = new User();
            if (!_repository.UserExists(model.UserId))
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
                                 .Where(x => x.UserId == model.UserId)
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

        public void DeleteUser(string userId)
        {
            var user = _repository.GetUsers().AsNoTracking()
                                 .Where(x => x.UserId == userId)
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

        public UserEditViewModel GetUser(string userId)
        {
            var user = _repository.GetUsers().AsNoTracking()
                                 .Where(x => x.UserId == userId)
                                 .Select(x => new UserEditViewModel
                                 {
                                     UserId = x.UserId,
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
                                    (string.IsNullOrEmpty(model.Filters.UserId) || x.UserId.ToLower().Contains(model.Filters.UserId.ToLower()))
                                    && (string.IsNullOrEmpty(model.Filters.Name) || x.Name.ToLower().Contains(model.Filters.Name.ToLower())));

                // DB is still not accessed yet
            }

            listModel.UserList = queryData
                                .Select(x => new UserViewModel
                                {
                                    UserId = x.UserId,
                                    Name = x.Name
                                }).ToList(); // Data is now retrieved from DB

            listModel.Filters = model?.Filters ?? new UserListViewModel.UserListFilterModel();

            return listModel;
        }
    }
}

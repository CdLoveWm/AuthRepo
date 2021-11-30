using Auth.IServices;
using Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auth.Services
{
    public class UserServices : IUserServices
    {
        private readonly List<User> users = new List<User>()
        {
            new User(){ UserCode = "123", UserName = "张三", Password = "123", Email = "123@qwe.com" },
            new User(){ UserCode = "456", UserName = "李四", Password = "456", Email = "456@qwe.com" },
            new User(){ UserCode = "789", UserName = "王五", Password = "789", Email = "789@qwe.com" },
        };


        public User FirstOrDefault(Func<User, bool> condition)
        {
            return users.FirstOrDefault(condition);
        }
    }
}

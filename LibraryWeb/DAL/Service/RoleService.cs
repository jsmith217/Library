using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryWeb.Models.Roles;
using LibraryWeb.Repository;

namespace LibraryWeb.Service
{
    public class RoleService : ICommonQueries<RoleModel>
    {
        private RoleRepository _roleRepo;

        public RoleService()
        {
            this._roleRepo = new RoleRepository();
        }

        public RoleModel GetAdminRole()
        {
            return this._roleRepo.Select(new List<string> { "Id=1" }).First();
        }

        public RoleModel GetRegularRole()
        {
            return this._roleRepo.Select(new List<string> { "Id=2" }).First();
        }

        public RoleModel GetById(int id)
        {
            return this._roleRepo.Select(new List<string> { $"Id={id}" }).First();
        }
    }
}
using Ehr.Data;
using Ehr.Interfaces;
using Ehr.Models;
using Ehr.Repository;
using System;

namespace Ehr.Bussiness
{
    public class UnitWork : IUnitWork
    {
        #region declare repository
        private EhrDbContext _context;
        private BaseRepository<User> _user;
        private BaseRepository<Role> _role;
        private BaseRepository<Permission> _permission;
        #endregion

        public UnitWork(EhrDbContext context)
        {
            _context = context;
        }

        #region get repository
        public IRepository<User> User => _user ?? (_user = new BaseRepository<User>(_context));
        public IRepository<Permission> Permission => _permission ?? (_permission = new BaseRepository<Permission>(_context));
        public IRepository<Role> Role => _role ?? (_role = new BaseRepository<Role>(_context));
        #endregion

        public void Commit()
        {
            _context.SaveChanges(); 
        }
    }
}
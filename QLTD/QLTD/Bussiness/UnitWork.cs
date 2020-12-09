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
        private QLTDDBContext _context;
        private BaseRepository<User> _user;
        private BaseRepository<Role> _role;
        private BaseRepository<Permission> _permission;
        private BaseRepository<Customer> _customer;
        private BaseRepository<Disk> _disk;
        private BaseRepository<DiskHold> _diskhold;
        private BaseRepository<DiskTitle> _disktitle;
        private BaseRepository<DiskType> _disktype;
        private BaseRepository<LateCharge> _latecharge;
        private BaseRepository<Order> _order;
        private BaseRepository<Rent> _rent;
        private BaseRepository<RentDetail> _rentdetail;
        private BaseRepository<RentReceipt> _rentreceipt;
        #endregion

        public UnitWork(QLTDDBContext context)
        {
            _context = context;
        }

        #region get repository
        public IRepository<User> User => _user ?? (_user = new BaseRepository<User>(_context));
        public IRepository<Permission> Permission => _permission ?? (_permission = new BaseRepository<Permission>(_context));
        public IRepository<Role> Role => _role ?? (_role = new BaseRepository<Role>(_context));
        public IRepository<Customer> Customer => _customer ?? (_customer = new BaseRepository<Customer>(_context));
        public IRepository<Disk> Disk => _disk ?? (_disk = new BaseRepository<Disk>(_context));
        public IRepository<DiskHold> DiskHold => _diskhold ?? (_diskhold = new BaseRepository<DiskHold>(_context));
        public IRepository<DiskTitle> DiskTitle => _disktitle ?? (_disktitle = new BaseRepository<DiskTitle>(_context));
        public IRepository<DiskType> DiskType => _disktype ?? (_disktype = new BaseRepository<DiskType>(_context));
        public IRepository<LateCharge> LateCharge => _latecharge ?? (_latecharge = new BaseRepository<LateCharge>(_context));
        public IRepository<Order> Order => _order ?? (_order = new BaseRepository<Order>(_context));
        public IRepository<Rent> Rent => _rent ?? (_rent = new BaseRepository<Rent>(_context));
        public IRepository<RentDetail> RentDetail => _rentdetail ?? (_rentdetail = new BaseRepository<RentDetail>(_context));
        public IRepository<RentReceipt> RentReceipt => _rentreceipt ?? (_rentreceipt = new BaseRepository<RentReceipt>(_context));
        #endregion

        public void Commit()
        {
            _context.SaveChanges(); 
        }
    }
}
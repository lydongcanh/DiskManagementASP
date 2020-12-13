using Ehr.Data;
using Ehr.Models;
using Ehr.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ehr.Interfaces
{
    /// <summary>
    /// Interface này dùng để khi mỗi khi mình add một lúc nhiều dbset sẽ giúp 
    /// nó thêm vô chung một context để không bị gây ra lỗi không thể add vô 
    /// hai context khác nhau
    /// </summary>
    public interface IUnitWork
    {
        IRepository<User> User { get; }
        IRepository<Permission> Permission { get; }
        IRepository<Role> Role { get; }
        IRepository<Customer> Customer { get; }
        IRepository<Disk> Disk { get; }
        IRepository<DiskHold> DiskHold { get; }
        IRepository<DiskTitle> DiskTitle { get; }
        IRepository<DiskType> DiskType { get; }
        IRepository<LateCharge> LateCharge { get; }
        IRepository<OrderLateCharge> Order { get; }
        IRepository<OrderRent> Rent { get; }
        IRepository<OrderDetail> RentDetail { get; }
        IRepository<OrderReceipt> RentReceipt { get; }
        void Commit();
    }
}

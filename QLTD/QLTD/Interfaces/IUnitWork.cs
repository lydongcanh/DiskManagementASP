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
        void Commit();
    }
}

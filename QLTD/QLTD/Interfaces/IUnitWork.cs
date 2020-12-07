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
        IRepository<AuditTrail> AuditTrail { get; }
        IRepository<LoginAuditTrail> LoginAuditTrail { get; }
        IRepository<ActionAuditTrail> ActionAuditTrail { get; }
        IRepository<Questionnaire> Questionnaire { get; }
        IRepository<ProductRequired> ProductRequired { get; }
        IRepository<AntimicrobialRequired> AntimicrobialRequired { get; }
        IRepository<Reference> Reference { get; }
        IRepository<ProductInfor> ProductInfor { get; }
        IRepository<Product> Product { get; }
        IRepository<AntimicroBial> AntimicroBial { get; }
        IRepository<OrtherAB> OrtherAB { get; }
        IRepository<Animal> Animal { get; }
        IRepository<AnimalInfor> AnimalInfor { get; }
        IRepository<Antimi> Antimi { get; }
        void Commit();
    }
}

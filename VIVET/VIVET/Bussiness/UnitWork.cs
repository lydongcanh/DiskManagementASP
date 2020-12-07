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
        private BaseRepository<AuditTrail> _audittrail;
        private BaseRepository<MailConfig> _mailconfig;
        private BaseRepository<LoginAuditTrail> _loginaudittrail;
        private BaseRepository<ActionAuditTrail> _actionaudittrails;
        private BaseRepository<Questionnaire> _questionnaire;
        private BaseRepository<ProductRequired> _productrequired;
        private BaseRepository<AntimicrobialRequired> _antimicrobialrequired;
        private BaseRepository<Reference> _reference;
        private BaseRepository<ProductInfor> _productinfor;
        private BaseRepository<Product> _product;
        private BaseRepository<AntimicroBial> _antimicrobial;
        private BaseRepository<OrtherAB> _ortherab;
        private BaseRepository<Animal> _animal;
        private BaseRepository<AnimalInfor> _animalinfor;
        private BaseRepository<Antimi> _antimi;
        #endregion

        public UnitWork(EhrDbContext context)
        {
            _context = context;
        }

        #region get repository
        public IRepository<User> User => _user ?? (_user = new BaseRepository<User>(_context));
        public IRepository<Permission> Permission => _permission ?? (_permission = new BaseRepository<Permission>(_context));
        public IRepository<Role> Role => _role ?? (_role = new BaseRepository<Role>(_context));
        public IRepository<MailConfig> MailConfig=> _mailconfig??(_mailconfig=new BaseRepository<MailConfig>(_context));
        public IRepository<AuditTrail> AuditTrail => _audittrail ?? (_audittrail = new BaseRepository<AuditTrail>(_context));
        public IRepository<LoginAuditTrail> LoginAuditTrail => _loginaudittrail ?? (_loginaudittrail = new BaseRepository<LoginAuditTrail>(_context));
        public IRepository<ActionAuditTrail> ActionAuditTrail => _actionaudittrails ?? (_actionaudittrails = new BaseRepository<ActionAuditTrail>(_context));
        public IRepository<Questionnaire> Questionnaire => _questionnaire ?? (_questionnaire = new BaseRepository<Questionnaire>(_context));
        public IRepository<ProductRequired> ProductRequired => _productrequired ?? (_productrequired = new BaseRepository<ProductRequired>(_context));
        public IRepository<AntimicrobialRequired> AntimicrobialRequired => _antimicrobialrequired ?? (_antimicrobialrequired = new BaseRepository<AntimicrobialRequired>(_context));
        public IRepository<Reference> Reference => _reference ?? (_reference = new BaseRepository<Reference>(_context));
        public IRepository<ProductInfor> ProductInfor => _productinfor ?? (_productinfor = new BaseRepository<ProductInfor>(_context));
        public IRepository<Product> Product => _product ?? (_product = new BaseRepository<Product>(_context));
        public IRepository<AntimicroBial> AntimicroBial => _antimicrobial ?? (_antimicrobial = new BaseRepository<AntimicroBial>(_context));
        public IRepository<OrtherAB> OrtherAB => _ortherab ?? (_ortherab = new BaseRepository<OrtherAB>(_context));
        public IRepository<Animal> Animal => _animal ?? (_animal = new BaseRepository<Animal>(_context));
        public IRepository<AnimalInfor> AnimalInfor => _animalinfor ?? (_animalinfor = new BaseRepository<AnimalInfor>(_context));
        public IRepository<Antimi> Antimi => _antimi ?? (_antimi = new BaseRepository<Antimi>(_context));
        #endregion

        public void Commit()
        {
            _context.SaveChanges(); 
        }
    }
}
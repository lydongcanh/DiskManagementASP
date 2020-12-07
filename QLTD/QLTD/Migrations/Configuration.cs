namespace Ehr.Migrations
{
    using Ehr.Auth;
    using Ehr.Common.Constraint;
    using Ehr.Data;
    using Ehr.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EhrDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

		void ReSeed ( EhrDbContext context )
		{
			

			#region Permission
			List<Permission> permissions = new List<Permission> ( );
			permissions.Add ( new Permission ( ) { PermissionCode = "CUS_MNT",PermissionName = "Quản lý khách hàng",PermisstionStatus = PermisstionStatus.ACTIVATED } );
			permissions.Add ( new Permission ( ) { PermissionCode = "DISK_MNT",PermissionName = "Quản lý đĩa",PermisstionStatus = PermisstionStatus.ACTIVATED } );
			permissions.Add ( new Permission ( ) { PermissionCode = "RENT_DISK",PermissionName = "Đặt/Thuê đĩa",PermisstionStatus = PermisstionStatus.ACTIVATED } );
			permissions.Add ( new Permission ( ) { PermissionCode = "USER_MNT",PermissionName = "Quản lý hệ thống",PermisstionStatus = PermisstionStatus.ACTIVATED } );
			context.Permissions.AddOrUpdate ( p => p.Id,permissions.ToArray ( ) );
			#endregion

			#region Role

			List<Role> roles = new List<Role> ( );
			roles.Add ( new Role ( ) { IsRoot = YesNo.YES,RoleName = "Admin",IntRole = UserRole.ADMIN,RoleStatus = RoleStatus.ACTIVATED,Permissions = permissions.GetRange ( 0,permissions.Count ) } );
			roles.Add ( new Role ( ) { IsRoot = YesNo.NO,RoleName = "Staff", IntRole = UserRole.ADMIN,RoleStatus = RoleStatus.ACTIVATED,Permissions = permissions.GetRange ( 2,permissions.Count - 2 ) } );
			context.Roles.AddOrUpdate ( r => r.Id,roles.ToArray ( ) );

			#endregion

			#region Users

			string _defPassword = "123456";
			List<User> users = new List<User> ( );
			users.Add ( new User ( ) { Username = "admin@gmail.com",Password = Utilities.Encrypt ( _defPassword ),IsRemember = false,IsActive = true,Roles = roles.GetRange ( 0,1 ),FullName = "Administrator"} );
			users.Add ( new User ( ) { Username = "staff@gmail.com",Password = Utilities.Encrypt ( _defPassword ),IsRemember = false,IsActive = true,Roles = roles.GetRange ( 0,1 ),FullName = "Staff" } );
			context.Users.AddOrUpdate ( u => u.Id,users.ToArray ( ) );

			#endregion
		}
		protected override void Seed(EhrDbContext context)
        {

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            ReSeed(context);
          
            /*
            try
            {
                var perActivated = new Permission
                {
                    PermisstionStatus = PermisstionStatus.ACTIVATED
                };
                var perNotActivated = new Permission
                {
                    PermisstionStatus = PermisstionStatus.NOTACTIVATED
                };

                context.Permissions.AddOrUpdate(p => p.Id, new Permission[] {
                    perActivated,
                    perNotActivated
                });

                var roleUser = new Role
                {
                    IntRole = UserRole.USER,
                    RoleName = "User",
                    Permissions = new List<Permission> { perActivated }
                };

                var roleEmp = new Role
                {
                    IntRole = UserRole.EMPLOYEE,
                    RoleName = "Employee",
                    Permissions = new List<Permission> { perActivated }
                };

                var roleManager = new Role
                {
                    IntRole = UserRole.MANAGER,
                    RoleName = "Manager",
                    Permissions = new List<Permission> { perActivated }
                };

                var roleAdmin = new Role
                {
                    IntRole = UserRole.ADMIN,
                    RoleName = "Administrator",
                    Permissions = new List<Permission> { perActivated }
                };

                context.Roles.AddOrUpdate(r => r.Id, new Role[] {
                    roleUser,
                    roleEmp,
                    roleManager,
                    roleAdmin
                });

                var userAdmin = new User
                {
                    Username = "admin@gmail.com",
                    Password = "uLIpkNIAl4U=",
                    IsRemember = false,
                    IsActive = true,
                    Roles = new List<Role> { roleAdmin }
                };

                var userManager = new User
                {
                    Username = "manager@gmail.com",
                    Password = "uLIpkNIAl4U=",
                    IsRemember = false,
                    IsActive = true,
                    Roles = new List<Role> { roleManager }
                };

                var userEmp = new User
                {
                    Username = "employee@gmail.com",
                    Password = "uLIpkNIAl4U=",
                    IsRemember = false,
                    IsActive = true,
                    Roles = new List<Role> { roleEmp },
                };

                var user = new User
                {
                    Username = "user@gmail.com",
                    Password = "uLIpkNIAl4U=",
                    IsRemember = false,
                    IsActive = true,
                    Roles = new List<Role> { roleUser },

                };

                context.Users.AddOrUpdate(u => u.Id, new User[]
                {
                    userAdmin,
                    userManager,
                    userEmp,
                    user
                });



                var admin = new Staff
                {
                    Email = "admin@gmail.com",
                    FullName = "Nguyễn Văn Hiền",
                    PhoneNumber = "0395252939",
                    Address = "170 Đặng Văn Bi",
                    Position = PositionStaff.MANAGER,
                    User = userAdmin
                };

                var manager = new Staff
                {
                    Address = "170 Đặng Văn Bi",
                    Email = "manager@gmail.com",
                    FullName = "Nguyễn Văn Hiền",
                    Position = PositionStaff.MANAGER,
                    PhoneNumber = "0395252939",
                    User = userManager
                };

                var employee = new Staff
                {
                    Address = "170 Đặng Văn Bi",
                    Email = "employee@gmail.com",
                    FullName = "Nguyễn Văn Hiền",
                    Position = PositionStaff.EMPLOYEE,
                    PhoneNumber = "0395252939",
                    User = userEmp
                };

                var userinfo = new Staff
                {
                    Address = "170 Đặng Văn Bi",
                    Email = "user@gmail.com",
                    FullName = "Nguyễn Văn Hiền",
                    Position = PositionStaff.EMPLOYEE,
                    PhoneNumber = "0395252939",
                    User = user
                };

                context.Staffs.AddOrUpdate(p => p.Id, new Staff[] {
                    admin,
                    manager,
                    employee,
                    userinfo
                });

                var customer = new Customer
                {
                    Logo = "",
                    Information = "",
                    Name = "SAM SUNG",
                    Phone = "0123456798",
                    RequirementStatus = RequirementStatus.HAD
                };

                context.Customers.AddOrUpdate(c => c.Id, new Customer[]
                 {
                        customer
                 });

                var region = new Region { RegionName = "Nam" };

                context.Regions.AddOrUpdate(r => r.Id, new Region[] {
                        region
                });

                var city = new City
                {
                    CityName = "HCM",
                    Region = region
                };
                context.Cities.AddOrUpdate(c => c.Id, new City[]
                {
                    city
                });

                context.Stores.AddOrUpdate(s => s.Id, new Store[] {
                    new Store{City=city,Phone="12345678", Name="Shop 1", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 2", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 3", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 4", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 5", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 6", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 7", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 8", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"},
                    new Store{City=city,Phone="12345678", Name="Shop 9", Code="123456", Customer=customer, District="Thu duc", StatusActive=ActiveStatus.ACTIVE, Street="Dang van bi", Ward="Binh tho"}
                });

                context.Vacancies.AddOrUpdate(v => v.Id, new Vacancy[] {
                    new Vacancy{Name = "VACANCY1", Note=""},
                    new Vacancy{Name = "VACANCY2", Note=""},
                    new Vacancy{Name = "VACANCY3", Note=""},
                    new Vacancy{Name = "VACANCY4", Note=""},
                    new Vacancy{Name = "VACANCY5", Note=""},
                    new Vacancy{Name = "VACANCY6", Note=""},
                    new Vacancy{Name = "VACANCY7", Note=""}
                 });

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

			*/
        }
    }
}

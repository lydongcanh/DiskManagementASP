# ehr

#khi checkout về mn nhớ sửa file Web.config lại nhé
<connectionStrings>
    <add name="DefaultConnection"
            connectionString="data source=112.78.1.49;initial catalog=HIEN_DEV;persist security info=True;user id=hiennv;password=****;multipleactiveresultsets=True;" providerName="System.Data.SqlClient"/>
</connectionStrings>

---> sửa ở initial catalog=<YOUR_DATA>, user id =<USER_LOGIN>, pass=<password> nha



# các thông tin đăng nhập. Mật khẩu sử dụng chung: 123456
1. User có quyền cao nhất: admin@gmail.com
2. nhân viên nhập dữ liệu: staff@gmail.com
3. Nhân viên xét duyêt: reviewer@gmail.com
4. Nhân viên quản lý: manager@gmail.com
5. User thông thường: user@gmail.com

#--------------Sửa lỗi khi migrattion 
1. Vào SQL database xóa bảng __MigrationHistory, Trong source c sharp xóa luôn các file trong Migration, chừa file Configuration ra
2. Nếu đã migrate rồi thì không cần data nữa, nên vào hàm Seed(EhrDbContext context) của Configuration thêm return; ở trước dòng ReSeed ( context );
3. Vào Package manager console chạy 2 lần lệnh sau: Add-Migration <Tên migration> (Ví dụ Add-Migration EZHR)
4. Tại Package manager console gọi update-database
#-------------------Tien's comments

Các bước cài đặt:
1.Cài AccessDatabaseEngine.exe
2. 
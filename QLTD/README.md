# ehr

#khi checkout về mn nhớ sửa file Web.config lại nhé
vô sqlserver tạo Database
<connectionStrings>
    <add name="DefaultConnection"
            connectionString="data source=112.78.1.49;initial catalog=TenDatabase;persist security info=True;user id=sa;password=sapassword;multipleactiveresultsets=True;" providerName="System.Data.SqlClient"/>
</connectionStrings>

---> sửa ở initial catalog=<YOUR_DATA>, user id =<USER_LOGIN>, pass=<password>  nha

mở Package manager console chạy lệnh:
add-migration DATTEN ( vd: add-migration ky_cd )
update-database
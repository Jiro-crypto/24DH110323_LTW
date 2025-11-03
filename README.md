# 24DH110323_MyStore
Cách để chạy không lỗi:
B1:Clone code về từ Git Hub bằng file ZIP
B2:Extract Folder từ ZIP ra máy (Lưu ý: Extract Folder bên trong ZIP thay vì Extract cả file ZIP để tránh lỗi ngoài ý muốn)
  Chạy Index.cshtml trong Categories hoặc Products để có thể thực thi các chức năng mượt mà nhất và tránh gặp lỗi
B3: Nếu khi bật file.cshtml lên mà bị Error thì bật Solution Explorer - chuột phải vào solution - chọn clean solution rồi bật lại Visual Studio
B4: Nếu vẫn bị lỗi thì có thể thử tắt VS rồi xóa "bin" và "obj" trong folder rồi bật code lại
Nếu không có "bin" và "obj" thì bật VS chạy code 1 lần thì sẽ xuất hiện. Tắt VS xóa "bin" và "obj" rồi bật lại VS sẽ có thể chạy code bình thường

Nếu không thấy Folder Categories hoặc Products thì có thể chỉnh branches từ "master" thành "BE3" hoặc "BE4" 


## 🔧 Cấu hình Database

1. Sau khi clone repo về, copy file `Web.config.template` thành `Web.config`.
2. Mở file `Web.config` và chỉnh lại `connectionString`:
   - `data source=`: SQL Server instance trên máy bạn (`localhost\SQLEXPRESS` hoặc tên máy).
   - `initial catalog=`: để nguyên `MyStoreDB`.
   - `user id` và `password`: nhập user/password SQL Server của bạn.
   - Hoặc nếu dùng Windows Authentication thì thay phần connection string bằng:
     ```
     data source=localhost\SQLEXPRESS;initial catalog=MyStoreDB;Integrated Security=True;
     ```

3. Import database:
   - Mở **SQL Server Management Studio (SSMS)**.
   - Chạy file `InitDatabase.sql` (có trong repo) để tạo database và dữ liệu mẫu.

4. Chạy project:
   - Mở Visual Studio.
   - Nhấn `F5` để chạy.

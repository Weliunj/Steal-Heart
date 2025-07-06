# Sửa lỗi Player Controls

## Các lỗi đã được sửa:

### 1. **Lỗi Movement (Di chuyển)**
- **Vấn đề**: Biến `move` không được gán giá trị input
- **Giải pháp**: Thêm `move = Input.GetAxis("Horizontal");` trong hàm `Update()`
- **Kết quả**: Player giờ có thể di chuyển bằng A/D hoặc Arrow keys

### 2. **Lỗi Attack (Tấn công)**
- **Vấn đề**: Các biến `attack1Input` và `attack2Input` đều được set là `false`
- **Giải pháp**: 
  - Attack 1: `Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)`
  - Attack 2: `Input.GetKeyDown(KeyCode.K) || Input.GetMouseButtonDown(1)`
- **Kết quả**: Player giờ có thể tấn công bằng J/K hoặc chuột trái/phải

### 3. **Lỗi Dash**
- **Vấn đề**: Biến `dashInput` được set là `false`
- **Giải pháp**: `Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)`
- **Kết quả**: Player giờ có thể dash bằng Shift

### 4. **Lỗi BOD variable**
- **Vấn đề**: Logic khởi tạo BOD không đúng
- **Giải pháp**: Sửa lại logic khởi tạo trong `Start()`

## Controls hiện tại:

| Action | Keyboard | Mouse |
|--------|----------|-------|
| **Move Left** | A, Left Arrow | - |
| **Move Right** | D, Right Arrow | - |
| **Jump** | Space | - |
| **Attack 1** | J | Left Click |
| **Attack 2** | K | Right Click |
| **Dash** | Left/Right Shift | - |
| **Use HP Potion** | H (hold for more HP) | - |

## Debug Features:
- Console sẽ hiển thị debug logs khi nhấn các phím movement
- Console sẽ hiển thị "Atk1 triggered!" khi tấn công thành công

## Kiểm tra:
1. Mở Unity Console (Window > General > Console)
2. Chạy game và thử các controls
3. Xem debug logs để đảm bảo input được nhận

## Lưu ý:
- Đảm bảo Player GameObject có đầy đủ components: Rigidbody2D, Animator, AudioSource[]
- Kiểm tra Input Manager settings trong Project Settings
- Đảm bảo các script được attach đúng vào Player GameObject 
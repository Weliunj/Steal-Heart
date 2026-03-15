# Steal-Heart (Unity 2D Action-RPG)

## 1. Giới thiệu

Steal-Heart is an action-adventure 2D game built in Unity.

## 2. Tính năng chính
- Di chuyển, nhảy (double jump), dash xuyên đối thủ.
- Ba kỹ năng tấn công:
  - Atk1, Atk2, Atk3 (dash + Atk2).
- Shop mua buff bằng Coin: HP bottle, Speed bottle, Jump bottle, Strength bottle.
- Boss BOD với nhiều pha, kỹ năng phóng đạn, spawn quái, teleport.
- Lưu/tiếp tục qua DataManager + save file.

## 3. Lối chơi
1. Bắt đầu từ menu chính (Play). 
2. Di chuyển qua map, tiến đến cổng Next để sang cảnh tiếp.
3. Đánh quái để nhận coin và item.
4. Vào khu vực Shop, nhấn shop để mở và mua bình.
5. Đến boss BOD; đánh trúng để giảm HP, boss sẽ chuyển pha khi còn HP thấp.
6. Thắng boss kết thúc demo (clear).

## 4. Điều khiển
- Nút trái/phải: di chuyển
- Nút Jump: nhảy
- Nút Atk1/Atk2: tấn công
- Nút Dash: dash
- Nút Heal (giữ): hồi HP
- Nút Speed/Jump/Strength: dùng buff
- Nút Shop/Inventory: mở UI

## 5. Cấu trúc chính
- `Assets/Scripts/Player/`: logic nhân vật, tấn công, UI.
- `Assets/Scripts/Enemies/`: AI, boss, enemy chung.
- `Assets/Scripts/Shop.cs`: shop mua item.
- `Assets/Scripts/DataManager.cs`: lưu giá trị qua scene.
- `Assets/Scripts/Menu.cs`: menu Play/Continue/Quit.

## 6. Chạy dự án
1. Mở Unity Hub, chọn project `e:\Project\Unity_Project\Steal-Heart`.
2. Mở scene menu chính (Menu).
3. Play trong Editor.

## 7. Tương lai
- Thêm checkpoint & save scene
- Cân bằng HP và boss
- Thêm nội dung map mới

---

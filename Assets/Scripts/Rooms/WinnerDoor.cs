//Khi người chơi chạm winnerdoor thì chiến thắng
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerDoor : MonoBehaviour
{
    [SerializeField] private GameObject WinUI;  // UI Canvas để hiển thị chiến thắng

    private void Awake()
    {
        // Đảm bảo UI chiến thắng ẩn khi bắt đầu game
        WinUI.SetActive(false);
    }

    // Hàm này được gọi khi một đối tượng có collider đi vào vùng trigger của cánh cửa
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng đi qua có phải là player không (giả định player có tag "Player")
        if (collision.CompareTag("Player"))
        {
            // Hiển thị UI chiến thắng
            WinUI.SetActive(true);

            // Tạm dừng game (nếu muốn)
            Time.timeScale = 0f;

            // Log thông điệp chiến thắng (optional)
            Debug.Log("You Win!");
        }
    }
}

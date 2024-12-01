//Quản lý các phòng trong trò chơi
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;// Mảng chứa các đối tượng kẻ thù trong phòng
    private Vector3[] initialPosition;// Mảng lưu trữ vị trí ban đầu của các kẻ thù

    private void Awake()
    {
        // Lưu trữ vị trí ban đầu của các kẻ thù
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)// Kiểm tra kẻ thù có tồn tại không
                initialPosition[i] = enemies[i].transform.position;// Lưu vị trí ban đầu
        }

        // Vô hiệu hóa phòng nếu nó không phải là phòng đầu tiên
        if (transform.GetSiblingIndex() != 0)
            ActivateRoom(false);
    }
    // Phương thức để kích hoạt hoặc vô hiệu hóa phòng
    public void ActivateRoom(bool _status)
    {
        // Kích hoạt hoặc vô hiệu hóa các kẻ thù trong phòng
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status); // Bật hoặc tắt enemy
                enemies[i].transform.position = initialPosition[i];// Đặt lại vị trí của enemy
            }
        }
    }
}
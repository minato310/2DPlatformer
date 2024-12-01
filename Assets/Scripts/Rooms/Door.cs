using UnityEngine;
using UnityEngine.SceneManagement; // Để dùng SceneManager

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;  // Tham chiếu đến phòng trước
    [SerializeField] private Transform nextRoom;      // Tham chiếu đến phòng tiếp theo
    [SerializeField] private CameraController cam;    // Tham chiếu đến CameraController để di chuyển camera
    [SerializeField] private bool isSceneDoor = false; // Cửa này có chuyển scene không?
    [SerializeField] private string targetSceneName;   // Tên scene cần chuyển (nếu là scene door)

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>(); // Lấy CameraController từ camera chính
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isSceneDoor) // Nếu đây là cửa để chuyển scene
            {
                ChangeScene();
            }
            else // Nếu đây là cửa chuyển giữa các phòng
            {
                ChangeRoom(collision);
            }
        }
    }

    private void ChangeRoom(Collider2D collision)
    {
        if (collision.transform.position.x < transform.position.x) // Người chơi ở bên trái cửa
        {
            cam.MoveToNewRoom(nextRoom);  // Di chuyển camera đến phòng tiếp theo
            nextRoom.GetComponent<Room>().ActivateRoom(true); // Kích hoạt phòng tiếp theo
            previousRoom.GetComponent<Room>().ActivateRoom(false); // Vô hiệu hóa phòng trước
        }
        else // Người chơi ở bên phải cửa
        {
            cam.MoveToNewRoom(previousRoom); // Di chuyển camera về phòng trước
            previousRoom.GetComponent<Room>().ActivateRoom(true); // Kích hoạt phòng trước
            nextRoom.GetComponent<Room>().ActivateRoom(false); // Vô hiệu hóa phòng tiếp theo
        }
    }

    private void ChangeScene()
    {
        Debug.Log("Đang chuyển scene sang: " + targetSceneName);
        SceneManager.LoadScene(targetSceneName); // Chuyển sang scene mới
    }
}

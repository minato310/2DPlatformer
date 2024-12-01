//Hồi sinh
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;//Biến này lưu âm thanh sẽ phát khi người chơi va chạm với điểm kiểm tra.
    private Transform currentCheckpoint;//Biến này lưu vị trí của điểm kiểm tra hiện tại mà người chơi đã đạt được.
    private Health playerHealth;//Biến này là một tham chiếu đến lớp Health, dùng để quản lý sức khỏe của người chơi.
    private UIManager uiManager;// Biến này giữ tham chiếu đến UIManager, để quản lý giao diện người dùng.

    private void Awake()
    {
        playerHealth = GetComponent<Health>();//Lấy tham chiếu đến thành phần Health trên đối tượng.
        uiManager = FindObjectOfType<UIManager>();//Tìm kiếm và lấy tham chiếu đến UIManager trong cảnh.
    }

    //Phương thức này được gọi để kiểm tra và thực hiện việc hồi sinh người chơi
    public void RespawnCheck()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        playerHealth.Respawn(); //Gọi phương thức Respawn() để khôi phục sức khỏe và đặt lại hoạt ảnh của người chơi.
        transform.position = currentCheckpoint.position; //Đặt vị trí của người chơi đến vị trí của điểm kiểm tra hiện tại.

        //Di chuyển camera đến khu vực tương ứng.
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }
    //Phương thức này được gọi khi đối tượng va chạm với một collider 2D.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")//Kiểm tra xem đối tượng va chạm có phải là một điểm kiểm tra không.
        {
            currentCheckpoint = collision.transform;// Nếu đúng, lưu vị trí của điểm kiểm tra vào biến currentCheckpoint.
            SoundManager.instance.PlaySound(checkpoint);//Phát âm thanh của điểm kiểm tra.
            collision.GetComponent<Collider2D>().enabled = false;//Vô hiệu hóa collider của điểm kiểm tra để tránh việc người chơi va chạm nhiều lần.
            collision.GetComponent<Animator>().SetTrigger("appear"); //Gọi trigger trên animator để thay đổi hoạt ảnh của điểm kiểm tra.
        }
    }
}
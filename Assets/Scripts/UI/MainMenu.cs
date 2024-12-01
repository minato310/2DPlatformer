using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;// Mũi tên di chuyển
    [SerializeField] private Button[] buttons;// Mảng các nút (Play và Quit)
    [SerializeField] private AudioClip changeSound; // Âm thanh khi di chuyển
    [SerializeField] private AudioClip interactSound;// Âm thanh khi tương tác
    private int currentPosition = 0;// Vị trí hiện tại của mũi tên

    private void Start()
    {
        // Đặt mũi tên ở vị trí nút đầu tiên
        UpdateArrowPosition();
        // Gán sự kiện OnClick cho tất cả các nút
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => Interact());
        }
    }

    private void Update()
    {
        // Kiểm tra phím mũi tên để di chuyển mũi tên
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);// Di chuyển lên trên
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1); // Di chuyển xuống dưới

        // Kiểm tra phím Enter hoặc Space để chọn nút hiện tại
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            Interact();// Gọi hàm xử lý tương tác
    }

    // Hàm thay đổi vị trí mũi tên giữa các nút
    private void ChangePosition(int direction)
    {
        currentPosition += direction;// Cập nhật vị trí hiện tại dựa trên hướng di chuyển

        // Giới hạn vị trí của mũi tên (chỉ có 2 nút)
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;// Nếu vị trí vượt qua nút đầu tiên, chuyển đến nút cuối cùng
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;// Nếu vị trí vượt qua nút cuối, quay lại nút đầu tiên

        // Phát âm thanh di chuyển
        if (direction != 0 && changeSound != null)
            AudioSource.PlayClipAtPoint(changeSound, Camera.main.transform.position);

        UpdateArrowPosition();// Cập nhật vị trí của mũi tên sau khi di chuyển
    }

    // Cập nhật vị trí mũi tên theo nút được chọn
    private void UpdateArrowPosition()
    {
        // Chuyển mũi tên tới vị trí y của nút được chọn
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].transform.position.y);
    }

    // Xử lý tương tác khi chọn một nút
    private void Interact()
    {
        //Phát âm thanh tương tác
        if (interactSound != null)
            AudioSource.PlayClipAtPoint(interactSound, Camera.main.transform.position);

        // Kiểm tra hành động dựa trên vị trí của mũi tên
        if (currentPosition == 0)
        {
            // Bắt đầu trò chơi (nút Play)
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        }
        else if (currentPosition == 1)
        {
            // Thoát trò chơi (nút Quit)
            Debug.Log("Game Quit");
            Application.Quit();
        }
    }

    // Hàm này để gọi khi click chuột vào nút
    public void OnButtonClicked(int buttonIndex)
    {
        currentPosition = buttonIndex;// Cập nhật vị trí theo nút đã nhấn
        Interact(); // Gọi hàm tương tác
    }

    // Hàm này được gọi khi di chuột qua nút
    public void OnButtonHover(int buttonIndex)
    {
        currentPosition = buttonIndex;// Cập nhật vị trí của mũi tên theo vị trí nút khi di chuột qua
        UpdateArrowPosition();// Cập nhật vị trí mũi tên
    }
}

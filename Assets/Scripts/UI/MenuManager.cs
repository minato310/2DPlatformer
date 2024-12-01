
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;// Mũi tên chỉ thị vị trí hiện tại của mũi tên
    [SerializeField] private RectTransform[] buttons;// Mảng các nút trong menu
    [SerializeField] private AudioClip changeSound;// Âm thanh khi di chuyển giữa các nút
    [SerializeField] private AudioClip interactSound;// Âm thanh khi chọn một nút
    private int currentPosition;//Chỉ số đại diện cho vị trí hiện tại của mũi tên trong menu

    private void Awake()
    {
        ChangePosition(0);// Đặt mũi tên tại vị trí của nút đầu tiên khi khởi động
    }
    private void Update()
    {
        // Kiểm tra phím mũi tên để di chuyển giữa các nút
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1);//Lên
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1);//Xuống
        // Kiểm tra phím Enter hoặc nút "Submit" để chọn mục hiện tại
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            Interact();
    }
    // Hàm thay đổi vị trí của mũi tên
    public void ChangePosition(int _change)
    {
        // Cập nhật vị trí hiện tại dựa trên giá trị `_change` (di chuyển lên/xuống)
        currentPosition += _change;
        // Nếu có sự thay đổi vị trí, phát âm thanh di chuyển
        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);
        // Đảm bảo vị trí của mũi tên nằm trong giới hạn số lượng nút
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;// Nếu vượt quá đầu, quay lại cuối
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;// Nếu vượt quá cuối, quay lại đầu

        AssignPosition();// Cập nhật vị trí của mũi tên
    }
    // Hàm cập nhật vị trí của mũi tên dựa trên nút được chọn
    private void AssignPosition()
    {
        // Cập nhật vị trí của mũi tên theo vị trí của nút hiện tại
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }
    // Hàm xử lý khi người dùng chọn một nút
    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound); // Phát âm thanh khi tương tác
        // Thực hiện hành động tương ứng với nút được chọn
        if (currentPosition == 0)
        {
            //Start game
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        }
        else if (currentPosition == 1)
        {
            //Open Settings
        }
        else if (currentPosition == 2)
        {
            //Open Credits
        }
        else if (currentPosition == 3)
            Application.Quit();
    }
}

//Quản lý di chuyển mũi tên chọn lựa trong một menu 
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;// Mảng các nút trong menu
    [SerializeField] private AudioClip changeSound;// Âm thanh khi di chuyển mũi tên
    [SerializeField] private AudioClip interactSound;// Âm thanh khi chọn một nút
    private RectTransform arrow;// Biến tham chiếu đến mũi tên di chuyển
    private int currentPosition;// Vị trí hiện tại của mũi tên trên menu

    private void Awake()
    {
        // Gán biến `arrow` với thành phần RectTransform của đối tượng hiện tại
        arrow = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        currentPosition = 0;// Đặt mũi tên ở vị trí nút đầu tiên khi menu xuất hiện
        ChangePosition(0);// Cập nhật vị trí mũi tên
    }
    private void Update()
    {
        // Di chuyển mũi tên khi người dùng nhấn phím mũi tên lên/xuống
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1);// Di chuyển lên
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1);// Di chuyển xuống

        //// Chọn nút hiện tại khi nhấn phím Enter
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();// Tương tác với nút hiện tại
    }
    // Hàm thay đổi vị trí của mũi tên
    private void ChangePosition(int _change)
    {
        currentPosition += _change;// Cập nhật vị trí hiện tại dựa trên giá trị truyền vào
        // Phát âm thanh khi có sự thay đổi vị trí
        if (_change != 0)
            SoundManager.instance.PlaySound(changeSound);
        // Giới hạn vị trí của mũi tên trong mảng nút
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;// Nếu vượt quá đầu danh sách, quay lại cuối
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;// Nếu vượt quá cuối danh sách, quay lại đầu

        AssignPosition();// Cập nhật vị trí mũi tên
    }
    // Cập nhật vị trí của mũi tên theo nút hiện tại
    private void AssignPosition()
    {
        // Đặt vị trí Y của mũi tên theo vị trí của nút hiện tại
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }
    // Xử lý khi người dùng tương tác với nút hiện tại
    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound);

        // Kích hoạt chức năng của nút hiện tại bằng cách gọi sự kiện `onClick` của nó
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}

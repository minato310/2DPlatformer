//thực hiện chức năng chuyển cảnh
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private void Update()
    {
        // Kiểm tra nếu người dùng nhấn phím F
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene(1);// Chuyển sang Scene có build index là 1
    }
}
//nhiệm vụ quản lý giao diện người dùng (UI) cho trò chơi
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;// Màn hình hiển thị khi trò chơi kết thúc
    [SerializeField] private AudioClip gameOverSound;// Âm thanh phát khi trò chơi kết thúc

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;// Màn hình tạm dừng trò chơi

    private void Awake()
    {
        // Khi trò chơi bắt đầu, ẩn cả màn hình Game Over và Pause
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    private void Update()
    {
        // Kiểm tra nếu người dùng nhấn phím Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Nếu màn hình Pause đang mở, đóng nó lại và ngược lại
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    #region Game Over
    // Hiển thị màn hình Game Over
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    // Khởi động lại cấp độ hiện tại
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Trở về màn hình chính
    public void MainMenu()
    {
        SceneManager.LoadScene(0);// Tải cảnh chính
    }

    // Thoát khỏi trò chơi
    public void Quit()
    {
        Application.Quit(); 

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Thoát khỏi chế độ Play trong Unity Editor
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        // Tạm dừng hoặc tiếp tục trò chơi dựa trên biến `status`
        pauseScreen.SetActive(status);// Kích hoạt hoặc vô hiệu hóa màn hình Pause
        // Nếu `status` là true, dừng thời gian (tạm dừng trò chơi), nếu false, tiếp tục thời gian (tiếp tục trò chơi)
        if (status)
            Time.timeScale = 0;// Dừng thời gian
        else
            Time.timeScale = 1;// Tiếp tục thời gian
    }
    // Điều chỉnh âm lượng âm thanh
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);// Thay đổi âm lượng âm thanh xuống 20%
    }
    // Điều chỉnh âm lượng nhạc
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);// Thay đổi âm lượng nhạc xuống 20%
    }
    #endregion
}

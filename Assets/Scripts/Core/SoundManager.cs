using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }// Biến static giữ duy nhất một tham chiếu tới đối tượng SoundManager. Điều này đảm bảo chỉ có một SoundManager trong game
    private AudioSource soundSource;//AudioSource để phát âm thanh (sound effects)
    private AudioSource musicSource;//AudioSource để phát nhạc nền (background music)


    private void Awake()
    {
        //Lấy tham chiếu đến AudioSource cho âm thanh và nhạc nền
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Giữ đối tượng SoundManager không bị hủy khi chuyển cảnh
        if (instance == null)
        {
            instance = this;//Gán thể hiện của SoundManager này làm instance duy nhất
            DontDestroyOnLoad(gameObject);//Không hủy đối tượng khi chuyển cảnh
        }
        //Nếu đã có một instance tồn tại, hủy đối tượng hiện tại để tránh trùng lặp
        else if (instance != null && instance != this)
            Destroy(gameObject);

        // Gán giá trị âm lượng ban đầu cho nhạc nền và âm thanh (mặc định là 0)
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    // Hàm phát âm thanh
    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }
    // Hàm thay đổi âm lượng của hiệu ứng âm thanh
    public void ChangeSoundVolume(float _change)
    {
        // Gọi hàm ChangeSourceVolume để thay đổi âm lượng cho soundSource
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }
    // Hàm thay đổi âm lượng của nhạc nền
    public void ChangeMusicVolume(float _change)
    {
        // Gọi hàm ChangeSourceVolume để thay đổi âm lượng cho musicSource
        ChangeSourceVolume(0.3f, "musicVolume", _change, musicSource);
    }
    // Hàm thay đổi âm lượng của một AudioSource (dùng chung cho nhạc nền và âm thanh)
    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);// Lấy giá trị âm lượng hiện tại từ PlayerPrefs (nếu không có, mặc định là 1)
        currentVolume += change; // Thay đổi giá trị âm lượng hiện tại dựa trên tham số truyền vào

        // Kiểm tra nếu âm lượng vượt quá 1 thì đặt về 0, ngược lại nếu nhỏ hơn 0 thì đặt về 1
        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        // Gán giá trị âm lượng cuối cùng sau khi thay đổi
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        // Lưu giá trị âm lượng hiện tại vào PlayerPrefs để sử dụng lại sau
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}
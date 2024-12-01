//Dùng để hiển thị giá trị âm lượng (volume) trên giao diện
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;// Tên của key lưu trong PlayerPrefs
    [SerializeField] private string textIntro; //Sound:  or Music:
    private Text txt; //Tham chiếu đến thành phần Text trong UI

    private void Awake()
    {
        txt = GetComponent<Text>();// Lấy thành phần Text trên đối tượng hiện tại
    }
    private void Update()
    {
        UpdateVolume();// Cập nhật giá trị âm lượng trên UI mỗi khung hình
    }
    private void UpdateVolume()
    {
        // Lấy giá trị âm lượng từ PlayerPrefs và nhân với 100 để đổi thành phần trăm
        float volumeValue = PlayerPrefs.GetFloat(volumeName) * 100;
        txt.text = textIntro + volumeValue.ToString();// Hiển thị giá trị âm lượng lên giao diện
    }
}
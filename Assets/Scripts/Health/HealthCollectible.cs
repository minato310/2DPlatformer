using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;//Số lượng máu mà người chơi sẽ nhận được khi thu thập vật phẩm
    [SerializeField] private AudioClip pickupSound;//Âm thanh sẽ phát ra khi người chơi thu thập vật phẩm

    //Hàm này được gọi khi một đối tượng khác (có thành phần Collider2D) đi vào vùng Trigger của vật phẩm này.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")//Kiểm tra xem đối tượng va chạm có phải là nhân vật người chơi hay không
        {
            SoundManager.instance.PlaySound(pickupSound);//Phát âm thanh khi người chơi thu thập vật phẩm
            collision.GetComponent<Health>().AddHealth(healthValue);//Truy cập thành phần Health của đối tượng người chơi (đối tượng va chạm) và gọi hàm AddHealth để thêm giá trị máu healthValue cho nhân vật.
            gameObject.SetActive(false);//Vô hiệu hóa đối tượng vật phẩm sau khi nó đã được thu thập, làm cho nó biến mất khỏi màn hình.
        }
    }
}
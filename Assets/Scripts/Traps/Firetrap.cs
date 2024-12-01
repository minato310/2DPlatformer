//Bẫy lửa có khả năng gây sát thương cho người chơi sau một khoảng thời gian kích hoạt
using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;//Lượng sát thương gây ra

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;// Thời gian chờ trước khi bẫy kích hoạt
    [SerializeField] private float activeTime;// Khoảng thời gian bẫy hoạt động và gây sát thương
    private Animator anim;// Animation cửa bãy
    private SpriteRenderer spriteRend;// Quản lý hình ảnh của bẫy

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;// Âm thanh khi bẫy lửa

    private bool triggered; // Xác định bẫy đã bị kích hoạt hay chưa
    private bool active; // Xác định bẫy đang hoạt động và có thể gây sát thương

    private Health playerHealth;// Tham chiếu đến sức khỏe của người chơi để gây sát thương

    private void Awake()
    {
        anim = GetComponent<Animator>();// Lấy Animator để điều khiển hoạt ảnh
        spriteRend = GetComponent<SpriteRenderer>();// Lấy SpriteRenderer để thay đổi màu của bẫy
    }
    // Phương thức Update kiểm tra xem bẫy có hoạt động và người chơi đang đứng trên bẫy không để gây sát thương liên tục
    private void Update()
    {
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage);// Gây sát thương nếu người chơi đứng trên bẫy khi bẫy đang hoạt động
    }
    // Phương thức OnTriggerEnter2D kiểm tra va chạm giữa người chơi và bẫy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();// Lấy tham chiếu đến thành phần sức khỏe của người chơi
            // Nếu bẫy chưa bị kích hoạt, bắt đầu quá trình kích hoạt bẫy
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
            // Nếu bẫy đang hoạt động, gây sát thương ngay lập tức
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
    //Phương thức OnTriggerExit2D dừng gây sát thương khi người chơi rời khỏi bẫy
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null;// Xóa tham chiếu đến thành phần sức khỏe khi người chơi rời bẫy
    }
    //điều khiển quá trình kích hoạt bẫy
    private IEnumerator ActivateFiretrap()
    {
        // Đổi màu sprite sang đỏ để cảnh báo người chơi rằng bẫy sắp kích hoạt
        triggered = true;
        spriteRend.color = Color.red;

        // Chờ thời gian kích hoạt, sau đó bật bẫy và phát âm thanh
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; // Đổi màu sprite
        active = true;
        anim.SetBool("activated", true);// Kích hoạt animation của bẫy

        // Sau thời gian hoạt động, tắt bẫy và đặt lại trạng thái ban đầu
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}

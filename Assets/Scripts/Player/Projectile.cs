//Quản lý hành vi của đạn ( quả cầu lửa, mũi tên)
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;//// Tốc độ của đạn
    private float direction;// Hướng di chuyển của đạn
    private bool hit;// Kiểm tra xem đã va chạm chưa
    private float lifetime;//Thời gian hoạt động của đạn

    //Tham chiếu
    private Animator anim;//Animation nổ
    private BoxCollider2D boxCollider;// Collider của đạn để xử lý va chạm

    private void Awake()
    {
        anim = GetComponent<Animator>();// Lấy Animator để điều khiển hoạt ảnh
        boxCollider = GetComponent<BoxCollider2D>();// Lấy Collider để xử lý va chạm
    }
    // Phương thức Update() được gọi mỗi khung hình
    private void Update()
    {
        if (hit) return;// Nếu đã va chạm thì dừng mọi hành động khác
        // Tính toán tốc độ di chuyển theo hướng và thời gian
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);// Di chuyển đạn theo hướng đã thiết lập
        //// Tăng thời gian sống của đạn và vô hiệu hóa đạn nếu quá 5 giây
        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }
    // Phương thức xử lý khi đạn va chạm với vật thể khác
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;// Đánh dấu đạn đã va chạm
        boxCollider.enabled = false;// Vô hiệu hóa collider để tránh va chạm tiếp
        anim.SetTrigger("explode");// Kích hoạt hoạt ảnh nổ
        // Nếu đạn va chạm với kẻ thù, gây sát thương cho kẻ thù
        if (collision.tag == "Enemy")
            collision.GetComponent<Health>().TakeDamage(1);
    }
    // Phương thức thiết lập hướng di chuyển của đạn
    public void SetDirection(float _direction)
    {
        lifetime = 0;// Đặt lại thời gian hoạt động của đạn
        direction = _direction; // Thiết lập hướng di chuyển
        gameObject.SetActive(true);// Kích hoạt đạn
        hit = false;// Đặt lại trạng thái va chạm
        boxCollider.enabled = true;// Kích hoạt collider để va chạm

        // Đặt lại hướng của đạn theo hướng di chuyển
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) // Nếu hướng hiện tại khác với hướng mới
            localScaleX = -localScaleX;//Đảo ngược hướng
        // Cập nhật kích thước để đạn nhìn đúng hướng
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    //Vô hiệu hóa đạn
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
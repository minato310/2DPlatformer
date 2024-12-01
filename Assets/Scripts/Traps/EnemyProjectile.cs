//Định nghĩa lớp EnemyProjectile, một loại đạn của kẻ địch trong game Unity. Nó kế thừa từ lớp EnemyDamage
using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;// Tốc độ di chuyển của đạn
    [SerializeField] private float resetTime;// Thời gian trước khi đạn biến mất nếu không trúng mục tiêu
    private float lifetime;// Thời gian tồn tại của đạn
    private Animator anim;// Animator để kiểm soát hiệu ứng 
    private BoxCollider2D coll;// Collider của đạn để kiểm soát va chạm

    private bool hit;// Kiểm tra xem đạn đã trúng mục tiêu hay chưa

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Lấy Animator từ đối tượng này
        coll = GetComponent<BoxCollider2D>();// Lấy Collider từ đối tượng này
    }
    // Kích hoạt đạn, khởi tạo lại trạng thái ban đầu của nó
    public void ActivateProjectile()
    {
        hit = false;// Đặt lại trạng thái chưa trúng mục tiêu
        lifetime = 0;// Đặt lại thời gian sống của đạn
        gameObject.SetActive(true);// Hiển thị đạn
        coll.enabled = true; // Kích hoạt Collider để đạn có thể va chạm
    }
    // Cập nhật vị trí của đạn và kiểm tra thời gian sống của nó
    private void Update()
    {
        if (hit) return;// Nếu đã trúng mục tiêu thì dừng di chuyển
        float movementSpeed = speed * Time.deltaTime;// Tính toán tốc độ di chuyển dựa trên thời gian
        transform.Translate(movementSpeed, 0, 0);// Di chuyển đạn theo hướng x

        lifetime += Time.deltaTime;// Tăng thời gian sống của đạn
        if (lifetime > resetTime)// Nếu vượt quá thời gian sống, đạn sẽ tự hủy
            gameObject.SetActive(false);
    }
    // Phương thức xử lý va chạm
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true; // Đạn đã trúng mục tiêu
        base.OnTriggerEnter2D(collision); // Gọi phương thức va chạm của lớp cha để gây sát thương cho người chơi
        coll.enabled = false;// Vô hiệu hóa Collider để tránh va chạm tiếp theo

        if (anim != null)
            anim.SetTrigger("explode"); // Kích hoạt hiệu ứng nổ
        else
            gameObject.SetActive(false); // Hủy đạn
    }
    // Vô hiệu hóa đạn
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
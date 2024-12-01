//Mô tả hành vi tấn công cận chiến của kẻ thù

using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;//Khoảng thời gian hồi chiêu giữa các đòn tấn công của kẻ thù.
    [SerializeField] private float range;//Phạm vi tấn công cận chiến của kẻ thù.
    [SerializeField] private int damage;//Sát thương mà kẻ thù gây ra khi tấn công.

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance; //Khoảng cách giữa kẻ thù và đối tượng phát hiện người chơi.
    [SerializeField] private BoxCollider2D boxCollider; //Collider của kẻ thù dùng để kiểm tra vị trí và kích thước cho việc phát hiện người chơi.

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;//Lớp của đối tượng người chơi, dùng để chỉ định đối tượng mà kẻ thù có thể phát hiện.
    private float cooldownTimer = Mathf.Infinity;//Bộ đếm thời gian để xác định khi nào kẻ thù có thể tấn công tiếp theo. Khởi tạo bằng giá trị Mathf.Infinity để kẻ thù có thể tấn công ngay từ đầu.

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;//Phát âm thanh khi tấn công
   

    private Animator anim;//Tham chiếu tới Animator để điều khiển các hoạt ảnh của kẻ thù.
    private Health playerHealth;//Tham chiếu tới hệ thống máu của người chơi để gây sát thương.
    private EnemyPatrol enemyPatrol;//Tham chiếu tới đối tượng EnemyPatrol, cho phép kẻ thù tuần tra khi không tấn công.

    //khởi tạo các tham chiếu đến Animator và EnemyPatrol.
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;//Tăng bộ đếm thời gian hồi chiêu của kẻ thù theo thời gian thực.

        //Kiểm tra xem người chơi có trong phạm vi tấn công hay không.
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth>0)
            {
                cooldownTimer = 0;//Đặt lại cooldownTimer về 0.
                anim.SetTrigger("meleeAttack");//Phát animation tấn công
                SoundManager.instance.PlaySound(attackSound);//Phát âm thanh tấn công
            }
        }
        //enemyPatrol sẽ được bật hoặc tắt dựa trên việc kẻ thù có nhìn thấy người chơi hay không. Nếu không nhìn thấy người chơi, kẻ thù tiếp tục tuần tra.
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }
    //Kiểm tra xem người chơi có trong phạm vi tấn công của kẻ thù không, bằng cách sử dụng BoxCast.
    private bool PlayerInSight()
    {
        //Tạo một hình hộp để phát hiện va chạm trong phạm vi tấn công của kẻ thù.
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        //Nếu BoxCast va chạm với người chơi (tức là hit.collider khác null), nó sẽ lưu tham chiếu đến hệ thống máu của người chơi (playerHealth).
        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    //Hàm này được sử dụng để vẽ một hình hộp đỏ (dùng để debug) trong Unity, giúp thấy được phạm vi phát hiện của kẻ thù.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    //Hàm này được gọi khi kẻ thù đánh trúng người chơi
    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);//nó sẽ gây sát thương lên người chơi bằng cách gọi hàm TakeDamage(damage) trên playerHealth.
    }
}

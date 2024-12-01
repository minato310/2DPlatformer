//Mô tả kẻ thù tấn công tầm xa
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;//Thời gian hồi chiêu giữa các đòn tấn công.
    [SerializeField] private float range;//Phạm vi phát hiện người chơi để thực hiện tấn công.
    [SerializeField] private int damage;//Lượng sát thương mà kẻ thù gây ra.

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;//Điểm nơi quả cầu lửa được phóng ra.
    [SerializeField] private GameObject[] fireballs;//Mảng chứa các đối tượng quả cầu lửa (fireball) mà kẻ thù sẽ sử dụng.

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;// Khoảng cách giữa kẻ thù và phạm vi để phát hiện người chơi.
    [SerializeField] private BoxCollider2D boxCollider;// Collider của kẻ thù, dùng để xác định kích thước và vị trí khi phát hiện người chơi.

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;//Lớp của đối tượng người chơi, giúp kẻ thù nhận diện đối tượng nào là người chơi.
    private float cooldownTimer = Mathf.Infinity;//Bộ đếm thời gian để xác định khi nào kẻ thù có thể thực hiện đòn tấn công tiếp theo. Khởi tạo với giá trị vô hạn để có thể tấn công ngay từ đầu.

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;//Âm thanh khi kẻ thù bắn ra quả cầu lửa.

    //Tham chiếu tới các thành phần
    private Animator anim;
    private EnemyPatrol enemyPatrol;

    //khởi tạo các tham chiếu tới Animator và EnemyPatrol của kẻ thù.
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;//Tăng bộ đếm thời gian hồi chiêu dựa trên thời gian thực.

        //Kiểm tra xem người chơi có nằm trong phạm vi phát hiện không
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");//khởi động animation tấn công rangedAttack.
            }
        }
        //Nếu không phát hiện người chơi, kẻ thù sẽ tiếp tục tuần tra bằng cách kích hoạt EnemyPatrol.
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    //Hàm này kích hoạt khi kẻ thù thực hiện tấn công tầm xa
    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);//Phát âm thanh quả cầu lửa
        cooldownTimer = 0;//Đặt lại thời gian hồi chiêu về 0
        fireballs[FindFireball()].transform.position = firepoint.position;//Tìm quả cầu lửa không sử dụng (FindFireball()) và đặt vị trí của nó tại firepoint.
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile();//Kích hoạt quả cầu lửa bằng cách gọi ActivateProjectile() trên đối tượng EnemyProjectile.
    }
    //Tìm và trả về chỉ số của quả cầu lửa không hoạt động trong mảng fireballs.Nếu tất cả các quả cầu lửa đều đang được sử dụng, nó sẽ trả về chỉ số đầu tiên.
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    //Sử dụng BoxCast để phát hiện người chơi trong phạm vi tấn công
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    //Hàm này được sử dụng để vẽ một hình hộp đỏ (dùng để debug) trong Unity, giúp thấy được phạm vi phát hiện của kẻ thù.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
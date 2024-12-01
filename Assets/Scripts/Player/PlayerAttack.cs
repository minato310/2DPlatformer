// Player tấn công bằng cách bắn ra quả cầu lửa
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;//Thời gian chờ giữa các lần tấn công
    [SerializeField] private Transform firePoint;//Vị trí mà cầu lửa sẽ được tạo ra
    [SerializeField] private GameObject[] fireballs;//Mảng các quả cầu lửa để nhân vật có thể bắn
    [SerializeField] private AudioClip fireballSound;//Âm thanh được phát khi nhân vật bắn cầu lửa

    private Animator anim;//Tham chiếu đến thành phần Animator của nhân vật để quản lý các animation tấn công.
    private PlayerMovement playerMovement;//Tham chiếu đến thành phần PlayerMovement để kiểm tra xem người chơi có thể tấn công hay không
    private float cooldownTimer = Mathf.Infinity;//Biến dùng để theo dõi thời gian chờ giữa các lần tấn công. Đặt là Mathf.Infinity để nhân vật có thể tấn công ngay từ đầu.

    //gọi khi đối tượng được khởi tạo. Nó gán các thành phần Animator và PlayerMovement để có thể sử dụng sau này.
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //Kiểm tra xem người chơi có nhấn chuột trái hay không,đã hết thời gian chờ giữa các lần tấn công chưa,Kiểm tra xem nhân vật có đủ điều kiện để tấn công không
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()
            && Time.timeScale > 0)
            Attack();//Tấn công

        cooldownTimer += Time.deltaTime;//cooldownTimer sẽ được cập nhật mỗi khung hình bằng Time.deltaTime để theo dõi thời gian chờ giữa các lần tấn công.
    }
    //Hàm tấn công
    private void Attack()
    {
        SoundManager.instance.PlaySound(fireballSound);//Phát âm thanh khi bắn cầu lửa.
        anim.SetTrigger("attack");//Kích hoạt animation tấn công
        cooldownTimer = 0;//Đặt lại thời gian chờ sau khi tấn công.

        fireballs[FindFireball()].transform.position = firePoint.position;//Di chuyển quả cầu lửa tới vị trí firePoint 
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));//Xác định hướng di chuyển của cầu lửa dựa trên hướng hiện tại của nhân vật (Mathf.Sign(transform.localScale.x)
                                                                                                              //Cho biết hướng mà nhân vật đang đối mặt: 1 hoặc -1)
    }
    //Hàm này tìm kiếm quả cầu lửa nào chưa được sử dụng (không hoạt động) để tái sử dụng nó. Nếu tất cả quả cầu lửa đang hoạt động, hàm sẽ trả về quả cầu lửa đầu tiên để sử dụng.
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
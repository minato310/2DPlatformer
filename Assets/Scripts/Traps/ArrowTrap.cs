//Bẫy mũi tên
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;// Thời gian chờ giữa các đợt tấn công
    [SerializeField] private Transform firePoint; // Vị trí mũi tên được bắn ra
    [SerializeField] private GameObject[] arrows;// Mảng chứa các mũi tên sẵn sàng bắn
    private float cooldownTimer;// Bộ đếm thời gian để theo dõi cooldown

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;// Âm thanh khi mũi tên được bắn

    private void Attack()
    {
        cooldownTimer = 0;// Reset bộ đếm thời gian

        SoundManager.instance.PlaySound(arrowSound);// Phát âm thanh khi bắn tên
        arrows[FindArrow()].transform.position = firePoint.position;// Đặt vị trí của mũi tên vào điểm bắn
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();// Kích hoạt mũi tên (cho phép nó bay)
    }
    // Tìm mũi tên chưa được sử dụng
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)// Nếu mũi tên không đang hoạt động
                return i;// Trả về chỉ số của mũi tên chưa sử dụng
        }
        return 0;
    }
    private void Update()
    {
        // Tăng bộ đếm thời gian theo thời gian đã trôi qua mỗi frame
        cooldownTimer += Time.deltaTime;
        // Nếu thời gian chờ đã đủ (lớn hơn hoặc bằng thời gian cooldown), bắn mũi tên
        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}
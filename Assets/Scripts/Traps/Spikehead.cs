//Bẫy có khả năng di chuyển nhanh về phía người chơi khi phát hiện ra họ
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;// Tốc độ di chuyển 
    [SerializeField] private float range;// Khoảng cách mà Spikehead có thể nhìn thấy người chơi
    [SerializeField] private float checkDelay;// Thời gian chờ giữa mỗi lần kiểm tra người chơi
    [SerializeField] private LayerMask playerLayer;// Layer để xác định người chơi
    private Vector3[] directions = new Vector3[4];// Mảng lưu trữ các hướng di chuyển
    private Vector3 destination;// Điểm đích mà Spikehead sẽ di chuyển tới
    private float checkTimer;// Bộ đếm thời gian giữa các lần kiểm tra người chơi
    private bool attacking;// Xác định nếu Spikehead đang tấn công người chơi

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;// Âm thanh khi Spikehead va chạm

    //Phương thức OnEnable được gọi khi đối tượng được kích hoạt
    private void OnEnable()
    {
        Stop();// Đặt trạng thái ban đầu cho Spikehead khi được kích hoạt
    }
    private void Update()
    {
        // Di chuyển Spikehead đến đích chỉ khi đang tấn công
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);// Di chuyển về hướng đích theo tốc độ định sẵn
        else
        {
            checkTimer += Time.deltaTime;// Tăng bộ đếm thời gian kiểm tra
            if (checkTimer > checkDelay)// Nếu thời gian đã vượt quá thời gian chờ kiểm tra
                CheckForPlayer();// Kiểm tra xem người chơi có trong phạm vi tấn công không
        }
    }
    // Phương thức CheckForPlayer kiểm tra người chơi trong các hướng cơ bản
    private void CheckForPlayer()
    {
        CalculateDirections();

        // Kiểm tra Spikehead có thấy người chơi trong 4 hướng không
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);// Vẽ tia kiểm tra 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);// Gửi tia kiểm tra va chạm
            // Nếu tìm thấy người chơi và Spikehead chưa tấn công
            if (hit.collider != null && !attacking)
            {
                attacking = true;// Bắt đầu tấn công
                destination = directions[i];// Đặt điểm đích là hướng mà người chơi được phát hiện
                checkTimer = 0;// Đặt lại bộ đếm kiểm tra
            }
        }
    }
    // Phương thức CalculateDirections tính toán các hướng di chuyển của Spikehead
    private void CalculateDirections()
    {
        directions[0] = transform.right * range; //Right 
        directions[1] = -transform.right * range; //Left 
        directions[2] = transform.up * range; //Up
        directions[3] = -transform.up * range; //Down
    }
    // Phương thức Stop dừng lại Spikehead
    private void Stop()
    {
        destination = transform.position; // Đặt điểm đích là vị trí hiện tại để ngừng di chuyển
        attacking = false;// Đặt trạng thái không tấn công
    }
    // Phương thức OnTriggerEnter2D xử lý va chạm
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(impactSound);// Phát âm thanh va chạm
        base.OnTriggerEnter2D(collision);// Gây sát thương nếu đối tượng va chạm là người chơi
        Stop(); // Dừng Spikehead sau khi va chạm
    }
}
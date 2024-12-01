using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;//Lượng máu ban đầu của nhân vật.
    public float currentHealth { get; private set; }//Lượng máu hiện tại
    private Animator anim;//Tham chiếu đến Animator để điều khiển hoạt ảnh của nhân vật
    private bool dead;//Biến xác định nhân vật đã chết hay chưa.

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;//Thời gian bất tử tạm thời sau khi nhận sát thương 
    [SerializeField] private int numberOfFlashes;//Số lần nhấp nháy của nhân vật khi bất tử.
    private SpriteRenderer spriteRend;//Để thay đổi màu nhân vật khi nhấp nháy trong thời gian bất tử

    [Header("Components")]
    [SerializeField] private Behaviour[] components;//Các thành phần khác liên kết với nhân vật (ví dụ: di chuyển, tấn công) sẽ bị vô hiệu hóa khi nhân vật chết.
    private bool invulnerable;//Biến cho biết nhân vật có đang ở trạng thái bất tử không.

    //Âm thanh khi nhân vật chể và bị đau
    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    //Hiển thị UI khi nhân vật chết
    [Header("UI Manager")]
    [SerializeField] private UIManager uiManager; // Tham chiếu đến UIManager

    //khởi tạo các giá trị cần thiết khi bắt đầu trò chơi
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    //Nhận sát thương với lượng sát thương _damage
    public void TakeDamage(float _damage)
    {
        //Nếu nhân vật đang ở trạng thái bất tử (invulnerable), bỏ qua sát thương.
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);//Giới hạn giá trị của máu không bao giờ vượt quá mức tối thiểu (0) và tối đa (máu ban đầu).
        //Nếu nhân vật còn sống kích hoạt animation hurt và bắt đầu trạng thái bất tử
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);//phát âm thanh bị thương
        }
        else
        {
            //Nếu nhân vật chết thì vô hiệu hóa tất cả các thành phần được đính kèm vào nhân vật
            if (!dead)
            {
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

                if (uiManager != null)
                {
                    uiManager.GameOver(); // Hiển thị Game Over UI
                }
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }
    //Hồi máu cho nhân vật với giá trị _value
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);//Sử dụng Mathf.Clamp() để đảm bảo máu không vượt quá mức tối đa
    }
    //Kích hoạt trạng thái bất tử tạm thời sau khi nhận sát thương
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);//Tạm thời vô hiệu hóa va chạm giữa nhân vật và các đối tượng khác trên các layer cụ thể (trong trường hợp này là layer 10 và 11).
        for (int i = 0; i < numberOfFlashes; i++)//Nhân vật sẽ nhấp nháy giữa màu đỏ và màu trắng với số lần nhấp nháy được xác định bởi numberOfFlashes
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);//Sau khi hết thời gian bất tử, nhân vật sẽ trở lại trạng thái bình thường
        invulnerable = false;
    }
    //Vô hiệu hóa đối tượng sau khi chết
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    //Tái sinh nhân vật, đặt lại máu về mức ban đầu và kích hoạt lại các thành phần bị vô hiệu hóa trước đó.
    public void Respawn()
    {
        AddHealth(startingHealth);
        //Reset trạng thái hoạt ảnh die và đặt lại hoạt ảnh idle
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invunerability());//Kích hoạt trạng thái bất tử tạm thời sau khi tái sinh
        //Kích hoạt lại các thành phần như chuyển động, tấn công,...
        foreach (Behaviour component in components)
            component.enabled = true;
    }
}
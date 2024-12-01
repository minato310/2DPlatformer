using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;//Tham chiếu đến thành phần Health của nhân vật, giúp truy cập lượng máu hiện tại của nhân vật.
    [SerializeField] private Image totalhealthBar;//Đối tượng Image đại diện cho thanh máu tổng thể 
    [SerializeField] private Image currenthealthBar;//Đối tượng Image đại diện cho thanh máu hiện tại

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;//Thiết lập lượng máu tổng thể
    }
    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.currentHealth/10;//Cập nhật thanh máu hiện tại dựa trên lượng máu hiện tại của nhân vật. Khi nhân vật mất máu, thanh này sẽ giảm tương ứng.
    }
}

using UnityEngine;

public class EnemyFireballHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()//Hàm này được Unity gọi mỗi frame. Do đó, bất cứ điều gì bạn đặt trong hàm này sẽ được thực thi liên tục trong quá trình chơi.
    {
        transform.localScale = enemy.localScale; //Câu lệnh này gán tỷ lệ cục bộ (localScale) của đối tượng hiện tại (đối tượng chứa script EnemyFireballHolder) theo tỷ lệ của enemy. 
                                                 // Tỉ lệ Fireball theo tỉ lệ enemy
    }
}
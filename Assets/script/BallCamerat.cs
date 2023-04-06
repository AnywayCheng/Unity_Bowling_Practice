using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallCamerat : MonoBehaviour
{
    public GameObject player; //母球
    private Vector3 offset;//攝影機 離 母球 相對位置
    public float speed = 12.0f; //母球 初速
    public float forceSpd = 9.0f; //母球 蓄力 速度
    private float force = 0.0f; //母球 蓄力大小
    public float distance = 6.0f; //攝影機 離 母球 距離 初始值    
    public float xSpeed = 120.0f; //滑鼠左右移動速度    
    public float ySpeed = 120.0f; //滑鼠上下移動速度
    public float yMinLimit = 20f;  //滑鼠上下 轉仰角 下限    
    public float yMaxLimit = 80f;   //滑鼠上下 轉仰角 上限
    public float distanceMin = 0.5f;  //滾輪 拉 攝影機 離 母球 距離下限    
    public float distanceMax = 15f;  //滾輪 拉 攝影機 離 母球 距離上限
    private Rigidbody rb;
    float x = 0.0f;
    float y = 0.0f;
    float low = 0.0f;
    bool stop = false;
    void Start()
    {
        offset = transform.position - player.transform.position; //攝影機位置 - 母球位置 = 相對位置
        Vector3 angles = transform.eulerAngles; //攝影機角度
        x = angles.y; //左右移動:繞母球（Y軸）轉, 瞄準四方
        y = angles.x; //前後移動:轉仰角（繞X軸轉）
        rb = player.GetComponent<Rigidbody>();  //取得剛體    
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        { //按住左鍵才作用
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f; //球轉
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f; //仰角轉
            y = ClampAngle(y, yMinLimit, yMaxLimit);  //限制 仰角傾仰範圍
            Quaternion rotation = Quaternion.Euler(y, x, 0); //根據新的x和y去調整z軸
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);// 限制滾輪拉遠近的移動範圍
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);//?????
            offset = rotation * negDistance; //依新角度，距離 重新算相對位置
            transform.rotation = rotation; // 攝影機 新角度
        }
        transform.position = player.transform.position + offset;
        if (Input.GetMouseButton(1) && stop == false)
        { //  按住右鍵蓄力
            force += Time.deltaTime * forceSpd; // 大小和時間成正比(每秒+9f)  
        }
        else if (Input.GetMouseButtonUp(1))
        { // 右鍵放開發射
            stop = true;
            Vector3 movement = Camera.main.transform.forward;//Camera.main.transform.forward是眼睛看的方向
            movement.y = 0.0f;      // no vertical movement 不上下移動
            rb.AddForce(movement * speed * force, ForceMode.Impulse);//ForceMode:動作模式 impulse:衝力，speed：初速大小
            force = 0.0f;  // 力量用盡，準備下次重新蓄力
            if (rb.velocity == Vector3.zero) //在完全停止前不能進行下次蓄力
            {
                stop = false;
            }
            if (player.transform.position.y < low)
            {
                transform.position = new Vector3(-9f, 0.25f, 0.0f);
            }

        }
        if (player.transform.position.z <= -6f )
        {
            Vector3 movement = new Vector3(-10.0f, 0.0f, -6f);
            speed = 3;
            rb.AddForce(movement * speed);
        }
        else if (player.transform.position.z >= 6f)
        {
            Vector3 movement = new Vector3(-10.0f, 0.0f, 6f);
            speed = 3;
            rb.AddForce(movement * speed);
        }
        if (player.transform.position.x <= -10f)
        {
            player.SetActive(false); //是的話將方塊Active設為false
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            player.transform.position = new Vector3(10f, 0.5f, 0.0f);
            player.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SceneManager.LoadScene(0);
        }

    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        if (angle > 360F)
        {
            angle -= 360F;
        }//把角度限制在正負360裡面
        return Mathf.Clamp(angle, min, max);//將angle的值調整到max跟min中間回傳回去
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BallCamerat : MonoBehaviour
{
    public GameObject player; //���y
    private Vector3 offset;//��v�� �� ���y �۹��m
    public float speed = 12.0f; //���y ��t
    public float forceSpd = 9.0f; //���y �W�O �t��
    private float force = 0.0f; //���y �W�O�j�p
    public float distance = 6.0f; //��v�� �� ���y �Z�� ��l��    
    public float xSpeed = 120.0f; //�ƹ����k���ʳt��    
    public float ySpeed = 120.0f; //�ƹ��W�U���ʳt��
    public float yMinLimit = 20f;  //�ƹ��W�U ����� �U��    
    public float yMaxLimit = 80f;   //�ƹ��W�U ����� �W��
    public float distanceMin = 0.5f;  //�u�� �� ��v�� �� ���y �Z���U��    
    public float distanceMax = 15f;  //�u�� �� ��v�� �� ���y �Z���W��
    private Rigidbody rb;
    float x = 0.0f;
    float y = 0.0f;
    float low = 0.0f;
    bool stop = false;
    void Start()
    {
        offset = transform.position - player.transform.position; //��v����m - ���y��m = �۹��m
        Vector3 angles = transform.eulerAngles; //��v������
        x = angles.y; //���k����:¶���y�]Y�b�^��, �˷ǥ|��
        y = angles.x; //�e�Ჾ��:������]¶X�b��^
        rb = player.GetComponent<Rigidbody>();  //���o����    
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        { //������~�@��
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f; //�y��
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f; //������
            y = ClampAngle(y, yMinLimit, yMaxLimit);  //���� �����ɥ��d��
            Quaternion rotation = Quaternion.Euler(y, x, 0); //�ھڷs��x�My�h�վ�z�b
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);// ����u���Ի��񪺲��ʽd��
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);//?????
            offset = rotation * negDistance; //�̷s���סA�Z�� ���s��۹��m
            transform.rotation = rotation; // ��v�� �s����
        }
        transform.position = player.transform.position + offset;
        if (Input.GetMouseButton(1) && stop == false)
        { //  ����k��W�O
            force += Time.deltaTime * forceSpd; // �j�p�M�ɶ�������(�C��+9f)  
        }
        else if (Input.GetMouseButtonUp(1))
        { // �k���}�o�g
            stop = true;
            Vector3 movement = Camera.main.transform.forward;//Camera.main.transform.forward�O�����ݪ���V
            movement.y = 0.0f;      // no vertical movement ���W�U����
            rb.AddForce(movement * speed * force, ForceMode.Impulse);//ForceMode:�ʧ@�Ҧ� impulse:�ĤO�Aspeed�G��t�j�p
            force = 0.0f;  // �O�q�κɡA�ǳƤU�����s�W�O
            if (rb.velocity == Vector3.zero) //�b��������e����i��U���W�O
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
            player.SetActive(false); //�O���ܱN���Active�]��false
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
        }//�⨤�׭���b���t360�̭�
        return Mathf.Clamp(angle, min, max);//�Nangle���Ƚվ��max��min�����^�Ǧ^�h
    }
}

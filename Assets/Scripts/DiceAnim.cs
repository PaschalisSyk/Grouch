using UnityEngine;
using DG.Tweening;

public class DiceAnim : MonoBehaviour
{
    [SerializeField] private Transform DiceWire, innerCube;
    [SerializeField] Color[] color;

    Outline outline;
    Material material;


    // Start is called before the first frame update
    void Start()
    {
        //outline = GetComponentInChildren<Outline>();
        material = DiceWire.GetComponent<MeshRenderer>().material;
        Shake();
    }

    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            Shake();
        }*/
    }

    public void Shake()
    {
        //DiceWire.DOShakeRotation(1.5f);
        //DiceWire.DOShakeScale(1.5f);
        transform.DOShakeRotation(1.5f,Random.Range(70,90),Random.Range(8,10),Random.Range(70,90));
        transform.DOShakeScale(1.5f);
        int colorNum = GameManager.instance.activePlayer;
        //outline.OutlineColor = Color.Lerp(outline.OutlineColor,color[colorNum], Mathf.PingPong(Time.time , 3));
        //material.color = color[colorNum];
        color[colorNum].a = 0.5f;
        material.color = color[colorNum];
        //Invoke("Scale", 2);

    }

    public void Scale()
    {     
        transform.DOScale(0, 1);
        Invoke("DestroyCube",2);
    }

    public void DestroyCube()
    {
            Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceNumberMesh : MonoBehaviour
{
    [SerializeField] Mesh[] mesh;
    //[SerializeField] GameObject[] gameObjects;
    [SerializeField] GameObject cube;
    [SerializeField] Material material;
    int dice;



    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = material;
        Mesh[] mesh = new Mesh[7];
        dice = GameManager.instance.dice;
        //_gameObject = Instantiate<GameObject>(gameObjects[dice],this.transform.position,transform.rotation);
        //_gameObject.transform.parent = cube.transform;

        //_gameObject.transform.localRotation = Quaternion.identity;
        //_gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<MeshFilter>().mesh = mesh[dice];
        gameObject.transform.LookAt(Camera.main.transform.position);
        //gameObject.transform.position = new Vector3(-10, 0, 2);
    }

}

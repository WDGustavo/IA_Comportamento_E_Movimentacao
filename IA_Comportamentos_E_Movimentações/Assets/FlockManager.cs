using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    //prefab do objeto que vai chamar e instanciar
    public GameObject fishPrefab;
    //quantidade de peixes a ser instanciado
    public int numFish= 20;
    //lista de piexes
    public GameObject[] allFish; 
    //limite de velocidade
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    //cria um local para o cardume usar como centro
    public Vector3 goalPos;

    //cria o local para controlar a velocidade
    [Header("Configurações do Cardume")]
    //limite para a velocidade minima
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    //limite para a velocidade maxima
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    //limita a distancia dos peixes
    [Range(1.0f, 10.0f)] 
    public float neighbourDistance;
    //limita a velocidade de rotação
    [Range(0.0f, 5.0f)] 
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //fala que a quantidade de objetos na lista vai ser o que esta no int
        allFish = new GameObject[numFish];
        //*instancia e seta o limite de velocidade do peixe
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x), 
                                                                Random.Range(-swinLimits.y, swinLimits.y), 
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //*
        //faz com que o cardume siga onde foi mandado para que ele não se perca
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //faz a movimentação do cardume para a posição setada
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
    }
}

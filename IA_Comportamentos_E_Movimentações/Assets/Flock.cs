using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{   //onde sera colocado o flockmanager
    public FlockManager myManager;
    //sua velocidade
    float speed;
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //chama a velocidade minima e maxima do flockingmanager
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        //seta o limite para os peixes ficarem na posição cetada
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        //cria o raycast para evitar a colisão com o pilar
        RaycastHit hit = new RaycastHit();
        //cria a direção que o objeto vai
        Vector3 direction = myManager.transform.position - transform.position;
        //*faz o calculo para não bater no pilar
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        //utiliza o raycast para impedir que os peixes colidão com o pilar
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            //faz os peixes refletirem
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else turning = false;
        //*
        if (turning)
        {
            //faz a rotação ser suave
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), 
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //caso seja menor que 10
            if (Random.Range(0,100)<10)
            {
                //pega a velocidade minima e maxima
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
            }
            //caso seja menor que 20
            if (Random.Range(0, 100) < 20)
            {
                //chama o metodo
                ApplyRules();
            }
        }
        //faz o peixe se mexer
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        //cria uma lista
        GameObject[] gos;
        //seta que os componentes dessa lista são os mesmo da lista de peixes
        gos = myManager.allFish;
        //calcula o centro entre os peixes
        Vector3 vcentre = Vector3.zero;
        //evita a colisão dos peixes
        Vector3 vavoid = Vector3.zero;
        //velocidade do cardume
        float gSpeed = 0.01f; 
        //guarda distancia do cardume
        float nDistance; 
        //Numero de peixes dentro dos grupos
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            //caso o go seja diferente do objeto
            if(go != this.gameObject)
            {
                //faz o calculo da distancia
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //faz o calculo da distancia dos vizinhos setata por mim
                if (nDistance <= myManager.neighbourDistance)
                {
                    //faz o calculo do vcentre atribuido no grupo
                    vcentre += go.transform.position;
                    groupSize++;
                    //se a distancia for menor quer 1 eles se separam
                    if (nDistance < 1.0f)
                    {
                        //evita a colisão e faz um novo calculo de movimentação
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //faz uma rotação para evitar a colisão
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;        
                }

            }
        }
        //caso esteja perto de um outro do grupo faz o cardume
        if (groupSize > 0)
        {
            //faz o centro ser o objeto para os peixes
            vcentre = vcentre / groupSize + (myManager.goalPos-this.transform.position);
            //faz a velocidade do grupo ser igual 
            speed = gSpeed / groupSize;
            //faz o calculo da direção
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if( direction != Vector3.zero)
            {
                //faz a rotação ser mais suave
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}


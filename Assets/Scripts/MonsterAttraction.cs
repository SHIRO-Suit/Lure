using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttraction : MonoBehaviour
{
    public Animator minigameAnim;
    public mouseControllCursor minigame;
    public AudioSource BreathingAS,UiAS;
    public float triggerDistance = 7 ;
    public bool antidoteInjected, isEscaping = false;
    float sqrDist, giveInSpeed = 1, progress, nearSpeed, ClosestSqrDist; 
    GameObject[] monsters;
    Transform nearestMonster;
    fpsController playerFpsCtrl;
    Pause playerPause;
    Vector3 direction ;
    bool giveInOrLose = false;
    Quaternion toRotation;
    
    // void OnDrawGizmosSelected() { // dessine les spheres dans l'editeur pour mieux se reperer, du coup ne marche pas puisque la recherche se fait dans la start seulement
    //     foreach (GameObject monster in monsters){
    //         Gizmos.DrawWireSphere(monster.transform.position,triggerDistance);
    //     }
        
    // }
    void Start()
    {
        playerFpsCtrl = GetComponent<fpsController>();
        playerPause = GetComponent<Pause>();
        sqrDist = Mathf.Pow(triggerDistance,2); // La puissance n'est pas directement calculée pour que je puisse Draw la sphere sans avoir a faire de racine carrée.
        monsters = GameObject.FindGameObjectsWithTag("Hallucination");
        foreach(GameObject monster in monsters){
            monster.GetComponent<AudioSource>().maxDistance = triggerDistance;
        }
    }

    void Update()
    {
        if(IsLured() && !isEscaping){
            

            //Calcul des directions et rotations            
            direction = (nearestMonster.position - transform.position);
            direction.y = 0;
            toRotation = Quaternion.LookRotation(direction,Vector3.up);

            if(antidoteInjected){
                nearSpeed= (sqrDist/direction.sqrMagnitude);// acceleration progressive lorse qu'on a le controle
                if(playerFpsCtrl.getMouse()!=Vector2.zero) progress = 0; // laisse le joueur deplacer son curseur et remet la vue sur le monstre lorsqu'on relache (avec antidote)
                deathWhenAntidote();
                nearestMonster.transform.rotation = toRotation ; // pour que le monstre nous fixe

            }else{
                nearSpeed = 1;
                minigameSequence(); // lance le minijeu seulement lorsqu'on a pas l'antidote.
                ControlsToogle(false); // desactive la camera et la possibilité de mettre pause
                nearestMonster.GetComponent<Animator>().Play("changeState");
            }
            
            //Application des mouvements
            progress += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation,progress*.5f); // rotation en Y du joueur vers le monstre
            fpsController.ThisCamera.transform.localRotation = Quaternion.Slerp(fpsController.ThisCamera.transform.localRotation,Quaternion.identity, progress); // rot X
            transform.Translate(direction.normalized * nearSpeed * giveInSpeed * Time.deltaTime * .6f, Space.World); // avancée vers le monstre, Give in speed est plus rapide quand on abandonne la resistance.
            
        }else if (isEscaping){
            nearestMonster.GetComponent<Animator>().Play("idle");
            BreathingPlay();
            toRotation *= (progress == 0 ) ? Quaternion.Euler(Vector3.up * 180) : Quaternion.identity;// fait le calcul qu'une seule fois pendant la fuite, evite de tourner a l'infini (demi-tour)
            progress += Time.deltaTime;
            
            transform.rotation = Quaternion.Slerp(transform.rotation,toRotation ,progress * 4); // rotation en Y vers L'opposé du monstre
            transform.Translate(-direction.normalized * Time.deltaTime * 5, Space.World); // fuite
            if(!IsLured()){
                isEscaping = false; // fin de la fuite quand on est plus dans le rayon d'action du monstre.
            }

        }else{
            if (!Pause.IsPaused){ // evite d'avoir le controle du fps et le menu en meme temps au demarrage
                ControlsToogle(true); 
            }
            
            
        }

    }
    public void Escape(){ // 
        isEscaping = true;
        SetAnimState(0);
        progress = 0;
    }
    void minigameSequence(){
        switch(GetAnimState()){
            case 0: if (!giveInOrLose && !isEscaping) {SetAnimState(1); nearestMonster.GetComponent<AudioSource>().Play();} break;
            case 1: Debug.Log("3"); 
            if(Input.GetKeyDown(KeyCode.R)){
                        SetAnimState(2);
                        UiAS.Play();
                        StartCoroutine(minigame.DirChange()); // lancement du minijeu
                    }else if(Input.GetKeyDown(KeyCode.G)){
                        giveInOrLose = true;
                        SetAnimState(0);
                        giveInSpeed = 2; // se reset tout seul puisque GiveIn signifie la mort, donc reset de la scene;
                        UiAS.Play();
                    }
                    break;
           //case 2: if(isEscaping) SetAnimState(0); break;
        }

            if(ClosestSqrDist < 2.25f){ //animation mort
                SetAnimState(3);
                Cursor.visible = true;
                this.enabled = false;
            }else if(ClosestSqrDist < 6.25f){ // si trop pres du monstre enleve l'ui, c'est perdu;
                giveInOrLose = true;
                SetAnimState(0);
            }// else if pour eviter conflit avec les deux SetAnimState(0);
        
    }
    void deathWhenAntidote(){ // pour pouvoir quand meme mourrir meme quand le minijeu ne tourne pas.
        if(ClosestSqrDist < 2.25f){ //animation mort
                ControlsToogle(false);
                SetAnimState(3);
                Cursor.visible = true;
                this.enabled = false;
            }
    }

    public int GetAnimState(){
        return minigameAnim.GetInteger("Step");
    }
    void SetAnimState(int step){
        minigameAnim.SetInteger("Step",step);
    }



    void ControlsToogle(bool state){
        playerPause.enabled = state;
        playerFpsCtrl.enabled = state;
    }
    
    bool IsLured(){
        return FindNearestMonster() < sqrDist;
    }

    float FindNearestMonster(){
        ClosestSqrDist = Mathf.Infinity;
        float Distance;
        foreach( GameObject monster in monsters){
            Distance = (monster.transform.position - transform.position).sqrMagnitude;
            if(Distance < ClosestSqrDist){
                nearestMonster = monster.transform;
                ClosestSqrDist = Distance;
            }
        }
        return ClosestSqrDist;
    }
    IEnumerator BreathingPlay(){
        BreathingAS.PlayOneShot(BreathingAS.clip);
        yield return new WaitForSeconds(2);
        BreathingAS.Stop();
    }
    public void injectAntidote(){
        antidoteInjected = true; // change le comportement de l'attraction;
        foreach(SwitchStateHalucination obj in GameObject.FindObjectsOfType<SwitchStateHalucination>()){
            obj.OnRealLife.Invoke(); // rends tout les monstres visibles normalement et bruyants.
        }
        
    }
}

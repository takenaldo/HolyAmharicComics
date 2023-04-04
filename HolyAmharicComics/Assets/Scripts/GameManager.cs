using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject current;


    public GameObject MainCamera;

    private Rigidbody2D cameraRB;

    public GameObject Page0;
    public GameObject Page1;
    public GameObject Page2;
    public GameObject Page3;
    public GameObject Page4;
    public GameObject Page5;
    public GameObject Page6;
    public GameObject Page7;
    public GameObject Page8;
    public GameObject Page9;
    public GameObject Page10;
    public GameObject Page11;
    public GameObject Page12;
    public GameObject Page13;
    public GameObject Page14;
    public GameObject Page15;
    public GameObject Page16;
    public GameObject Page17;
    public GameObject Page18;
    public GameObject Page19;
    public GameObject Page20;

    public GameObject[] Pages;

    private bool action_move_prev = false;
    private bool action_move_next = false;

    private bool action_move = false;

    private bool action_zoom = false;
    private bool appear = false;


    private int step = 0;

    private UserMovement input;
    private bool swiping = false;


    // Start is called before the first frame update
    void Start()
    {
        input = gameObject.GetComponent<UserMovement>();
        cameraRB = MainCamera.GetComponent<Rigidbody2D>();
        Pages = new GameObject[20] { Page0, Page1, Page2, Page3, Page4, Page5, Page6, Page7, Page8, Page9, Page10, Page11, Page12, Page13, Page14, Page15, Page16, Page17, Page18, Page19 };
    }

    // Update is called once per frame
    void Update()
    {
       
        if (input.SwipRight || input.SwipeDown || Input.GetKeyDown("d") && swiping == false)
        {
            if (!isEndingPage())
            {
                //            current = current.GetComponent<NavigationController>().next;
                //            toNextStep();
                GameObject next = current.GetComponent<NavigationController>().next;
                PlayAudioFor(next);
                StopAllAudiosExceptFor(next);
                action_move_next = true;
                swiping = true;
            }
        }

        else if (input.SwipeLeft || input.SwipeUp || Input.GetKeyDown("a") && swiping == false)
        {
            if (!isStartingPage())
            {
                //current = current.GetComponent<NavigationController>().prev;
                //toPreviousStep();
                PlayAudioFor(current.GetComponent<NavigationController>().prev);
                if(current.GetComponent<NavigationController>().dontStopPreviousAudios is false)
                    StopAllAudiosExceptFor(current.GetComponent<NavigationController>().prev);
                action_move_prev = true;

                swiping = true;
            }
        }
        else
        {
            swiping = false;
        }


        if (action_move_next == true)
        {
            MoveFocusTo(current.GetComponent<NavigationController>().next.transform.position);
        }
        else if(action_move_prev == true)
        {
            MoveFocusTo(current.GetComponent<NavigationController>().prev.transform.position);
        }



        return;











        if (input.SwipRight || input.SwipeDown || Input.GetKeyDown("d"))
        {
            Debug.Log("Swiped");
            // Next
            if (swiping == false)
            {
                swiping = true;
                step++;
            }

            if (step == 4 || step == 5 || step == 16)
                StopAllAudios();

            if (step == 6)
            {
                ZoomOut(Pages[step]);
                action_move = false;
                action_zoom = true;
            }



            action_move = true;
            Debug.Log("==========> " + step);
            // play the prpared audio for the particular page
            PlayAudioFor(Pages[step]);

            /*            if(step == 3)
                        {
                            Pages[step - 1].GetComponent<SpriteRenderer>().spriteSortPoint = SpriteSortPoint.Pivot;
                            Pages[step-1].transform.localScale = new Vector3(Pages[step-1].transform.localScale.x / 2, Pages[step-1].transform.localScale.y, Pages[step-1].transform.localScale.z);
                            return;
                        }*/
            //if (step == 4)
            //{

            //                return;
            //}
            //else
            //{
            //      action_move = true;
            //}
        }
        else if (input.SwipeLeft || input.SwipeUp || Input.GetKeyDown("a"))
        {
            // Prev
        }
        else
        {
            swiping = false;
        }


        //if (Input.GetKeyDown("a") || Input.GetMouseButtonDown(0))
        /*if (Input.GetKeyDown("a") || input.SwipRight )

        {
            if(swiping == false)
            {
                swiping = true;
                step++;
            }

            if (step == 1)
            {
                playAudio();
            }
            else if (step == 2)
            {
                AudioSource []  audioSources =   Page2.GetComponents<AudioSource>();
                for(int i=0; i<audioSources.Length; i++)
                {
                    audioSources[i].Play();
                }
            }
            Debug.Log(gameObject.name);
            action_move = true;
            Debug.Log("A clicked");
        }*/
        /*        else
                {
                    swiping = false;
                }*/

        if (action_move || appear)
        {
            Debug.Log("moving to " + step);
            if (step == 4)
            {
                Vector3 newPos = Pages[step - 1].transform.position;
                newPos.y = Pages[step - 1].transform.position.y - 2f;
                MoveTo(Pages[step], newPos);
            }
            else if (step == 10)
            {
                Vector3 newPos = Pages[step - 1].transform.position;
                newPos.y = Pages[step - 1].transform.position.y - 2f;
                MoveTo(Pages[step], newPos);
            }

            else
            {
                MoveFocusTo(Pages[step].transform.position);
            }
        }

        if (action_zoom)
        {
            if (step == 6)
            {
                float x = Pages[step].transform.localScale.x + 0.1f;
                float y = Pages[step].transform.localScale.y + 0.1f;

                if (x <= 2f && y <= 1.5f)
                    Pages[step].transform.localScale = new Vector3(x, y, 1);
                else
                    action_zoom = false;
            }
        }

        //        Debug.Log(step);
    }

    private void StopAudiosFor(GameObject go)
    {

        GameObject[] audiosToStop = go.GetComponent<NavigationController>().audiosToStop;


        for(int i=0; i<audiosToStop.Length; i++)
        {
            AudioSource[] AllAudios = go.GetComponents<AudioSource>();
            foreach (AudioSource audio in AllAudios)
                if (audio.isPlaying)
                    audio.Stop();


        }
    }

    private bool isEndingPage()
    {
        Debug.Log(current.tag);
        return current.CompareTag("end");
    }

    private bool isStartingPage()
    {
        Debug.Log(current.tag);
        return current.CompareTag("start");
    }
    // go to next step
    private void toNextStep()
    {
        current = current.GetComponent<NavigationController>().next;
        MoveFocusTo(current.transform.position);
        PlayAudioFor(current);
    }

    // go to previous step

    private void toPreviousStep()
    {
        current = current.GetComponent<NavigationController>().prev;
        MoveFocusTo(current.transform.position);
        PlayAudioFor(current);
    }

    private void ZoomOut(GameObject gameObject)
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    private void StopAllAudios()
    {
        AudioSource[] AllAudios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in AllAudios)
            if (audio.isPlaying)
                audio.Stop();
    }

    private void StopAllAudiosExceptFor(GameObject go)
    {
        AudioSource[] AllAudios = GameObject.FindObjectsOfType<AudioSource>();
        int i = 0;
        foreach (AudioSource audio in AllAudios)
        {
            Debug.Log("===> "+i);
            if (audio.gameObject != go && audio.isPlaying)
            {
                Debug.Log(">>>>>>>>>>>>>>>");
                audio.Stop();
                
            }
        }
    }

    private void PlayAudioFor(GameObject Page)
    {
        AudioSource[] audioSources = Page.GetComponents<AudioSource>();
        Debug.Log("==> " + audioSources.Length);
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying == false)
                audioSources[i].Play();
        }
    }

    private void FixedUpdate()
    {

    }



    public void OldMoveCameraTo(Vector3 direction, GameObject destinationPage)
    {

        if (action_move)
        {



            bool reached = false;
            if (direction == Vector3.up || direction == Vector3.down)
                reached = ((int)MainCamera.transform.position.y == (int)destinationPage.transform.position.y);
            else if (direction == Vector3.left || direction == Vector3.right)
                reached = ((int)MainCamera.transform.position.x == (int)destinationPage.transform.position.x);

            int mX = (int)MainCamera.transform.position.x;
            int mY = (int)MainCamera.transform.position.y;

            int dX = (int)destinationPage.transform.position.x;
            int dY = (int)destinationPage.transform.position.y;

            if (mX == dX)
            {
                if ((int)mY == (int)dY)
                    reached = true;

                else if (mY > dY)
                    direction = Vector3.down;

                else if (mY < dY)
                    direction = Vector3.up;

            }
            else if (mY == dY)
            {
                if ((int)mX == (int)dX)
                    reached = true;

                else if (mX > dX)
                    direction = Vector3.left;
                else if (mX < dX)
                    direction = Vector3.right;


            }


            //reached = ((int)MainCamera.transform.position.x == (int)destinationPage.transform.position.x) && ((int)MainCamera.transform.position.y == (int)destinationPage.transform.position.y);

            /*            Debug.Log("dest ");
                        Debug.Log("x : " + destinationPage.transform.position.x);
                        Debug.Log("y : " + destinationPage.transform.position.y);
            */

            if (reached)
            {
                action_move = false;
            }
            else
            {
                //                MainCamera.transform.Translate(direction);
                //                MainCamera.transform.Translate(destinationPage.transform.position, Space.World);

                float x = MainCamera.transform.position.x - destinationPage.transform.position.x;
                float y = MainCamera.transform.position.y - destinationPage.transform.position.y;

                Debug.Log("X = " + x);
                Debug.Log("Y = " + y);
                Debug.Log("-------------------------");

                //                MainCamera.transform.Translate(x, y, 0);

                Vector3 a = transform.position;
                Vector3 b = destinationPage.transform.position;

                MainCamera.transform.position = Vector3.MoveTowards(a, b, 0.1f);

            }
        }

    }



    public void MoveFocusTo(Vector3 destination)
    {
        float diffX = Math.Abs(MainCamera.transform.position.x - destination.x);
        float diffY = Math.Abs(MainCamera.transform.position.y - destination.y);

        Debug.Log("X : " + destination.x);
        Debug.Log("Y : " + destination.y);


        if (diffX < Math.Abs(0.3f) && diffY < Math.Abs(0.3f))
        {
            if (action_move_next == true)
                current = current.GetComponent<NavigationController>().next;
            else if (action_move_prev == true)
                current = current.GetComponent<NavigationController>().prev;

            action_move_next = false;
            action_move_prev = false;

            Debug.Log("Reached");
        }
        else
        {
            Vector3 a = MainCamera.transform.position;
            Vector3 b = destination;
            destination.z = -10;
            MainCamera.transform.position = Vector3.Lerp(a, destination, 0.1f);

        }
    }


    public void MoveTo(GameObject obj, Vector3 destination)
    {
        float diffX = Math.Abs(obj.transform.position.x - destination.x);
        float diffY = Math.Abs(obj.transform.position.y - destination.y);

        Debug.Log("Diff x " + diffX);
        Debug.Log("Diff y " + diffY);

        if (diffX < Math.Abs(0.3f) && diffY < Math.Abs(0.3f))
        {
            action_move = false;
            Debug.Log("Reached");
        }
        else
        {
            Vector3 a = obj.transform.position;
            Vector3 b = destination;
            //            destination.z = -10;
            obj.transform.position = Vector3.Lerp(a, destination, 0.1f);

        }
    }
}


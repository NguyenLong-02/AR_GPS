using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GoShared;
using TMPro;

public class ARObjectUpdate : MonoBehaviour
{
    [SerializeField] Transform ArCamera;
    [SerializeField] Transform Player; // Player_Positon
    [SerializeField] GameObject Cube; // AR Object Position                   

    [SerializeField] TextMeshProUGUI text_pos_player;
    [SerializeField] TextMeshProUGUI text_pos_object;
    [SerializeField] TextMeshProUGUI _distance;

    [SerializeField] List<GameObject> object_list;
    [SerializeField] List<GameObject> object_list_avaliable;
    [SerializeField] List<GameObject> object_list_avaliable_AR;

    [SerializeField] Coordinates Object_to_Place;// AR Object RealWorld Position

    float object_radius = 3f; // Radius of Object to Appear When Player Entering
    // Start is called before the first frame update
    void Start()
    {
        Object_to_Place.longitude = 106.687092f;
        Object_to_Place.latitude = 10.805243f;
        StartCoroutine(UpdateHandle());

        ArCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        if (object_list == null) object_list = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        text_pos_object.text = Object_to_Place.description();// RealWorld AR Object Position

        if (object_list.Count > 0)
        {
            foreach (var _object in object_list)   // Search Object In List Of AR Object
            {
                float distance = Vector3.Distance(Player.position, _object.transform.position);  // For Checking Distance Between Player and AR Object
                Debug.Log("distance : " + distance);

                _distance.text = ""+distance;    // Distance Of Object and Player In Unity Coordinate 

                Vector3 direction = _object.transform.position - Player.position;
                Coordinates currentLocation = Coordinates.convertVectorToCoordinates(Player.transform.position);
                text_pos_player.text = "" + currentLocation.description(); // RealWorld Player Position
                Debug.Log("direction : " + direction);
                Debug.Log("Coordinates : " + currentLocation.latitude +"\n" + currentLocation.longitude);


                // Whenever Player Entering The Object Radius Then Instantiate The Object In Front Of AR Camera
                if (distance <= object_radius)
                {
                    GameObject _object_clone = _object;
                    if (!object_list_avaliable.Contains(_object_clone)) // If AR Object is not Contain in List Then =>  Add and Instantiate
                    {
                        object_list_avaliable.Add(_object_clone); // Add Current Object to List For Checking In Next Update Frame

                        // Creat The Vector 3 Vector Scale For Scaling Clone of AR Object in MapBox To Instantiate In Front Of AR Camera With It Scale
                        Vector3 Scale = new Vector3(0.1f, 0.1f, 0.1f);

                        // Instantiate In Front Of AR Camera With Valid Direction Depend On Direction Of Object And Player In MapBox
                        // ArCamera.position + direction/80 Because The Object In MapBox is At Scale(8,8,8) And When I Set It With Scale(0.1f,0.1f,0.1f) It's Reduce 80 times
                        // So The Direction Also Must Reduce 80 times
                        GameObject newGame_object = Instantiate(_object_clone, ArCamera.position + direction/80, ArCamera.rotation); 

                        // After Instantiate In Front Of AR Camera Set The Scale For Valid Size With RealWorld
                        newGame_object.transform.localScale = Scale;

                        object_list_avaliable_AR.Add(newGame_object); // After That Add AR Object To The List
                    }
                }else
                {
                    // When Distance Of Object And Player Is More Than Object Radius Destroy It In RealWorld (Destroy The Object In Front Of AR Camera)
                    object_list_avaliable_AR.ForEach((obj) => Destroy(obj.gameObject));
                    object_list_avaliable.Clear(); 
                }
            }
        }
    }

    IEnumerator UpdateHandle()
    {
        yield return new WaitForSeconds(1f); // Wait 1 seconds for loading map then set the AR Object Depend On Longtitude and Latitude

        // Created New Unity Coordinates Follow RealWorld Position Of Object
        Coordinates coordinates = new Coordinates(Object_to_Place.latitude, Object_to_Place.longitude, 0);
        Cube.transform.localPosition = coordinates.convertCoordinateToVector(0);
        // After Setting Position Of Unity Coordinates Set the Vector3.Y of Object Equal With Player So We Can See The Object On MiniMap
        Cube.transform.position = new Vector3(Cube.transform.position.x, Player.position.y, Cube.transform.position.z);
    }
}

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
    [SerializeField] Transform Player;
    [SerializeField] GameObject Cube;

    [SerializeField] TextMeshProUGUI text_pos_player;
    [SerializeField] TextMeshProUGUI text_pos_object;
    [SerializeField] TextMeshProUGUI _distance;

    [SerializeField] List<GameObject> object_list;
    [SerializeField] List<GameObject> object_list_avaliable;
    [SerializeField] List<GameObject> object_list_avaliable_AR;

    [SerializeField] Coordinates Object_to_Place;

    float object_radius = 3f;
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
        text_pos_object.text = Object_to_Place.description();

        if (object_list.Count > 0)
        {
            foreach (var _object in object_list)
            {
                float distance = Vector3.Distance(Player.position, _object.transform.position);
                Debug.Log("distance : " + distance);
                _distance.text = ""+distance;

                Vector3 direction = _object.transform.position - Player.position;
                Coordinates currentLocation = Coordinates.convertVectorToCoordinates(Player.transform.position);
                text_pos_player.text = "" + currentLocation.description();
                Debug.Log("direction : " + direction);
                Debug.Log("Coordinates : " + currentLocation.latitude +"\n" + currentLocation.longitude);

                if (distance <= object_radius)
                {
                    GameObject _object_clone = _object;
                    if (!object_list_avaliable.Contains(_object_clone))
                    {
                        object_list_avaliable.Add(_object_clone);
                        Vector3 Scale = new Vector3(0.1f, 0.1f, 0.1f);
                        GameObject newGame_object = Instantiate(_object_clone, ArCamera.position + direction/80, ArCamera.rotation);
                        newGame_object.transform.localScale = Scale;

                        object_list_avaliable_AR.Add(newGame_object);
                    }
                }else
                {
                    object_list_avaliable_AR.ForEach((obj) => Destroy(obj.gameObject));
                    object_list_avaliable.Clear();
                }
            }
        }
    }

    IEnumerator UpdateHandle()
    {
        yield return new WaitForSeconds(1f);
        Coordinates coordinates = new Coordinates(Object_to_Place.latitude, Object_to_Place.longitude, 0);
        Cube.transform.localPosition = coordinates.convertCoordinateToVector(0);
        Cube.transform.position = new Vector3(Cube.transform.position.x, Player.position.y, Cube.transform.position.z);
    }
}

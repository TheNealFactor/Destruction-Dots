using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedPieceController : MonoBehaviour
{

    public TriggerOptionsDOTS triggerOptions;

    public bool is_connected = true;
    [HideInInspector] public bool visited = false;
    public List<DestroyedPieceController> connected_to;

    public static bool is_dirty = false;

    private Rigidbody _rigidbody;
    private Vector3 _starting_pos;
    private Quaternion _starting_orientation;
    private Vector3 _starting_scale;

    private bool _configured = false;
    private bool _connections_found = false;
    Material _material;

    //Shrink Scale of Objects
    Vector3 temp;
    private float changeSpeed = 2f;
    bool trigger = false;

    // Start is called before the first frame update
    void Start()
    {
        connected_to = new List<DestroyedPieceController>();
        _starting_pos = transform.position;
        _starting_orientation = transform.rotation;
        _starting_scale = transform.localScale;
        _material = GetComponent<Renderer>().material;

        transform.localScale *= 1.02f;

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (trigger)
        {
            StartCoroutine(ScaleObjectSize());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (!_configured)
        {
            var neighbour = collision.gameObject.GetComponent<DestroyedPieceController>();
            if (neighbour)
            {
                if (!connected_to.Contains(neighbour))
                    connected_to.Add(neighbour);
            }
        }
        else if (collision.gameObject.CompareTag("Floor"))
        {
            //VFXController.Instance.spawn_dust_cloud(transform.position);
        }
        if(collision.gameObject.CompareTag("Projectile"))
        {
            //collision.collider.GetComponent<DestroyedPieceController>().cause_damage(transform.forward);
           // Debug.Log("Projectile hit Statue");
            cause_damage(transform.forward);
        }

        if(collision.gameObject.CompareTag("Floor") && is_dirty)
        {
            trigger = true;
        }
    }

    public void make_static()
    {
        _configured = true;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = true;

        transform.localScale = _starting_scale;
        transform.position = _starting_pos;
        transform.rotation = _starting_orientation;
    }

    public void cause_damage(Vector3 force)
    {
        is_connected = false;
        _rigidbody.isKinematic = false;
        is_dirty = true;
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _material.color = Color.red;
        //VFXController.Instance.spawn_dust_cloud(transform.position);
    }

    public void drop()
    {
        is_connected = false;
        _rigidbody.isKinematic = false;
    }

   public IEnumerator ScaleObjectSize()
    {
        Debug.Log("ScaleObject");
        Destroy(_rigidbody);

        yield return new WaitForSeconds(5);
        if (temp.x >= 0)
        {
            temp = transform.localScale;
            temp.x -= 1f * changeSpeed * Time.deltaTime;
            temp.y -= 1f * changeSpeed * Time.deltaTime;
            temp.z -= 1f * changeSpeed * Time.deltaTime;
            transform.localScale = temp;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
